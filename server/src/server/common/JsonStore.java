package server.common;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;

import java.io.*;
import java.lang.reflect.Type;
import java.nio.ByteBuffer;
import java.nio.charset.CharacterCodingException;
import java.nio.charset.Charset;
import java.nio.charset.CodingErrorAction;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.HashMap;
import java.util.Map;

public class JsonStore {
    private static final Gson GSON = new GsonBuilder().setPrettyPrinting().create();
    private final String path;
    private Map<String, Object> data = new HashMap<>();

    public JsonStore(String name) {
        this.path = Config.DATA_DIR + "/" + name + ".json";
        load();
    }

    /** 老数据是 GBK 编码，新写入统一 UTF-8；先严格按 UTF-8 解，失败再按 GBK。 */
    private static String decode(byte[] bytes) {
        try {
            return StandardCharsets.UTF_8.newDecoder()
                    .onMalformedInput(CodingErrorAction.REPORT)
                    .onUnmappableCharacter(CodingErrorAction.REPORT)
                    .decode(ByteBuffer.wrap(bytes)).toString();
        } catch (CharacterCodingException e) {
            return new String(bytes, Charset.forName("GBK"));
        }
    }

    /**
     * 加载存档。顺序：主文件 -> .bak 备份 -> 都不行则把坏文件改名留档，从空开始。
     * 以前这里解析失败会静默变成空数据，下一次保存就把玩家的存档彻底覆盖掉，代价太大。
     */
    private void load() {
        File f = new File(path);
        if (f.exists() && tryLoad(path)) return;
        if (!f.exists() && !new File(path + ".bak").exists()) {
            data = new HashMap<>();
            return;
        }
        File bak = new File(path + ".bak");
        if (bak.exists() && tryLoad(bak.getPath())) {
            System.err.println("[JsonStore] recovered from backup: " + bak.getName());
            save();
            return;
        }
        if (f.exists()) {
            File corrupt = new File(path + ".corrupt-" + System.currentTimeMillis());
            if (f.renameTo(corrupt)) {
                System.err.println("[JsonStore] unrecoverable, kept as " + corrupt.getName());
            }
        }
        data = new HashMap<>();
    }

    private boolean tryLoad(String p) {
        try {
            File f = new File(p);
            if (!f.exists()) return false;
            Type t = new TypeToken<Map<String, Object>>() {}.getType();
            Map<String, Object> m = GSON.fromJson(decode(Files.readAllBytes(Paths.get(p))), t);
            if (m == null) return false;
            data = m;
            return true;
        } catch (Exception e) {
            System.err.println("[JsonStore] load failed (" + p + "): " + e.getMessage());
            return false;
        }
    }

    public synchronized Object get(String key) {
        return data.get(key);
    }

    public synchronized void reload() {
        load();
    }

    public synchronized void put(String key, Object value) {
        data.put(key, value);
        save();
    }

    /**
     * 原子保存：先写同目录 .tmp 并 fsync，再把旧文件备份成 .bak，最后原子替换主文件。
     * 直接覆盖写会在写到一半时被中断（进程被杀/断电），留下截断的 JSON，
     * 之前就是这样把 roles.json 写成 24KB 的半截文件，角色直接读不出来。
     */
    public synchronized void save() {
        File f = new File(path);
        File tmp = new File(path + ".tmp");
        File bak = new File(path + ".bak");
        try {
            f.getParentFile().mkdirs();
            try (FileOutputStream fos = new FileOutputStream(tmp)) {
                Writer w = new OutputStreamWriter(fos, StandardCharsets.UTF_8);
                GSON.toJson(data, w);
                w.flush();
                fos.getFD().sync();
            }
            if (f.exists()) {
                Files.copy(f.toPath(), bak.toPath(), StandardCopyOption.REPLACE_EXISTING);
            }
            Files.move(tmp.toPath(), f.toPath(),
                    StandardCopyOption.ATOMIC_MOVE, StandardCopyOption.REPLACE_EXISTING);
        } catch (Exception e) {
            System.err.println("[JsonStore] save failed: " + e.getMessage());
        }
    }

    public Map<String, Object> all() {
        return data;
    }
}
