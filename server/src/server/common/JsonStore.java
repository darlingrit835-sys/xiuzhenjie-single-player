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
import java.nio.file.AtomicMoveNotSupportedException;
import java.nio.file.Path;
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

    private void load() {
        try {
            File f = new File(path);
            if (f.exists()) {
                Type t = new TypeToken<Map<String, Object>>() {}.getType();
                data = GSON.fromJson(decode(Files.readAllBytes(Paths.get(path))), t);
                if (data == null) data = new HashMap<>();
            }
        } catch (Exception e) {
            throw new IllegalStateException("无法读取存档 " + path, e);
        }
    }

    public synchronized Object get(String key) {
        return data.get(key);
    }

    public synchronized void reload() {
        load();
    }

    public synchronized void put(String key, Object value) {
        Object previous = data.put(key, value);
        try {
            save();
        } catch (RuntimeException error) {
            if (previous == null) data.remove(key);
            else data.put(key, previous);
            throw error;
        }
    }

    public synchronized void save() {
        Path target = Paths.get(path);
        Path temp = null;
        try {
            Files.createDirectories(target.getParent());
            temp = Files.createTempFile(target.getParent(), target.getFileName().toString(), ".tmp");
            try (Writer w = Files.newBufferedWriter(temp, StandardCharsets.UTF_8)) {
                GSON.toJson(data, w);
            }
            try {
                Files.move(temp, target, StandardCopyOption.ATOMIC_MOVE, StandardCopyOption.REPLACE_EXISTING);
            } catch (AtomicMoveNotSupportedException e) {
                Files.move(temp, target, StandardCopyOption.REPLACE_EXISTING);
            }
            temp = null;
        } catch (IOException e) {
            throw new UncheckedIOException("无法写入存档 " + path, e);
        } finally {
            if (temp != null) {
                try { Files.deleteIfExists(temp); } catch (IOException ignored) { }
            }
        }
    }

    public Map<String, Object> all() {
        return data;
    }
}
