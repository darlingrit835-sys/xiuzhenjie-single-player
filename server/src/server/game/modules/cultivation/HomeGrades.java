package server.game;

import com.google.gson.Gson;
import server.common.Config;

import java.io.IOException;
import java.math.BigDecimal;
import java.nio.ByteBuffer;
import java.nio.charset.CharacterCodingException;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Map;

final class HomeGrades {
    static final int MAX_GRADE = 4;

    private HomeGrades() {}

    static int grade(Map<String, Object> role) {
        return Math.max(0, Math.min(MAX_GRADE, RoleUtil.num(role, "DongFuRank")));
    }

    static int wireRank(int grade) {
        return grade * 4;
    }

    static String homeName(Map<String, Object> role) {
        return String.valueOf(role.getOrDefault("Name", "道友")) + "的洞府";
    }

    static long upgradePrice(int grade, int vipLevel) {
        long base = Prices.BY_GRADE[grade];
        return vipLevel >= 4 ? base * 9 / 10 : base;
    }

    private static final class Prices {
        static final long[] BY_GRADE = loadPrices();
    }

    private static final class ConfiguredHome {
        String id;
        int level;
        BigDecimal price;
    }

    private static long[] loadPrices() {
        Path path = Paths.get(Config.DATA_DIR, "config", "xzjworldhomelist.json");
        long[] prices = new long[MAX_GRADE + 1];
        try {
            byte[] bytes = Files.readAllBytes(path);
            String json;
            try {
                json = StandardCharsets.UTF_8.newDecoder().decode(ByteBuffer.wrap(bytes)).toString();
            } catch (CharacterCodingException invalidUtf8) {
                json = new String(bytes, Charset.forName("GBK"));
            }
            ConfiguredHome[] homes = new Gson().fromJson(json, ConfiguredHome[].class);
            if (homes == null) return prices;
            for (ConfiguredHome home : homes) {
                if (home == null || home.id == null || home.id.length() != 1 || home.price == null) continue;
                int grade = home.id.charAt(0) - '0';
                if (grade < 1 || grade > MAX_GRADE || home.level != wireRank(grade)) continue;
                if (home.price.signum() > 0 && home.price.compareTo(BigDecimal.valueOf(Integer.MAX_VALUE)) <= 0) {
                    prices[grade] = home.price.longValueExact();
                }
            }
        } catch (IOException | RuntimeException invalidConfig) {
            System.err.println("[Home] Unable to load configured upgrade prices");
            return new long[MAX_GRADE + 1];
        }
        return prices;
    }

}
