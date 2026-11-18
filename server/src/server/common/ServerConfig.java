package server.common;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;

/**
 * 私服可调参数。放在 data/config/server.json，改完重启即可。
 */
public class ServerConfig {
    private static final Gson GSON = new GsonBuilder().setPrettyPrinting().create();
    private static ServerConfig INSTANCE;

    /** 修炼速度倍率。1.0 = 按设计表节奏，10.0 = 十倍速。 */
    public double cultivationSpeed = 1.0;
    /** 下发给客户端的服务器公网/局域网 IP。 */
    public String publicIp = Config.PUBLIC_IP;
    /** 离线收益最多结算多少小时。 */
    public int offlineCapHours = 12;
    /** 战斗服端口。 */
    public int battlePort = 17667;
    /** 单场战斗时长（秒）。 */
    public int battleDurationSeconds = 600;
    /** 每场战斗刷多少只怪。 */
    public int monsterCount = 6;
    /** 怪物整体强度倍率：0.7 简单 / 1.0 标准 / 1.3 困难。 */
    public double monsterDifficulty = 0.85;
    /** 每只怪的经验/灵石系数。 */
    public double dropMultiplier = 1.0;
    /**
     * 战斗引导是否直接视为已完成（默认开启）。
     * 客户端 BattleWnd.LiKaiBtnClick 有个硬拦截：BattleGuideVal != 100 且新手指引步数 < 16 时
     * 不允许离开战斗地图。私服没有完整引导触发链，关闭会导致玩家被困在战斗地图里。
     */
    public boolean completeBattleGuide = true;

    public static synchronized ServerConfig get() {
        if (INSTANCE == null) INSTANCE = load();
        return INSTANCE;
    }

    private static ServerConfig load() {
        String path = Config.DATA_DIR + "/config/server.json";
        try {
            File f = new File(path);
            if (f.exists()) {
                String json = new String(Files.readAllBytes(Paths.get(path)), StandardCharsets.UTF_8);
                ServerConfig c = GSON.fromJson(json, ServerConfig.class);
                if (c != null) {
                    c.normalize();
                    return c;
                }
            }
        } catch (Exception e) {
            System.err.println("[ServerConfig] load failed: " + e.getMessage());
        }
        ServerConfig c = new ServerConfig();
        c.save(path);
        return c;
    }

    private void normalize() {
        if (cultivationSpeed <= 0) cultivationSpeed = 1.0;
        if (publicIp == null || publicIp.trim().isEmpty()) publicIp = Config.PUBLIC_IP;
        if (offlineCapHours <= 0) offlineCapHours = 12;
        if (battlePort <= 0) battlePort = 17667;
        if (battleDurationSeconds <= 0) battleDurationSeconds = 600;
        if (monsterCount <= 0) monsterCount = 6;
        if (monsterDifficulty <= 0) monsterDifficulty = 1.0;
        if (dropMultiplier <= 0) dropMultiplier = 1.0;
    }

    private void save(String path) {
        try {
            File f = new File(path);
            f.getParentFile().mkdirs();
            try (Writer w = new OutputStreamWriter(new FileOutputStream(f), StandardCharsets.UTF_8)) {
                GSON.toJson(this, w);
            }
        } catch (Exception e) {
            System.err.println("[ServerConfig] save failed: " + e.getMessage());
        }
    }
}
