package server.game;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import server.common.Config;

import java.io.File;
import java.lang.reflect.Type;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 玄天楼商品表（原版在客户端的 AssetBundle 里，这里解出来落到
 * data/config/xuantianlou_goods.json，服务端才能真正扣款并发货）。
 */
public class XuanTianLouService {

    public static class Goods {
        public int Id;
        public String Name;
        public String TypeName;
        public String TemplateId;
        public int Rank;
        public int Level;
        public int Stage;
        public int GoodsType;
        public int XTL_GoodsType;
        public int BuyType;   // 1=灵石 2=灵玉
        public int GoodsPrice;
        public int FiveElement;
        public String GoodsDescribe;
        public List<String> GoodsDetail;
    }

    private static final Map<Integer, Goods> TABLE = new LinkedHashMap<>();
    private static final List<Integer> HOT = new ArrayList<>();
    private static boolean loaded;

    public static synchronized void ensureLoaded() {
        if (loaded) return;
        loaded = true;
        // 两个文件名都认，方便不同来源的包直接跑
        File f = new File(Config.DATA_DIR + "/config/xuantianlou_goods.json");
        if (!f.exists()) f = new File(Config.DATA_DIR + "/config/xuantianlougoods.json");
        if (!f.exists()) {
            System.out.println("[XuanTianLou] goods table missing: " + f.getPath());
            return;
        }
        try {
            String text = new String(Files.readAllBytes(f.toPath()), StandardCharsets.UTF_8);
            Type t = new TypeToken<List<Goods>>() {}.getType();
            List<Goods> list = new Gson().fromJson(text, t);
            if (list == null) return;
            for (Goods g : list) {
                if (g == null || g.Id <= 0) continue;
                TABLE.put(g.Id, g);
                if (HOT.size() < 4 && g.XTL_GoodsType == 1) HOT.add(g.Id);
            }
            System.out.println("[XuanTianLou] loaded " + TABLE.size() + " goods");
        } catch (Exception e) {
            System.out.println("[XuanTianLou] load failed: " + e.getMessage());
        }
    }

    public static Map<Integer, Goods> all() {
        ensureLoaded();
        return TABLE;
    }

    public static Goods get(int id) {
        ensureLoaded();
        return TABLE.get(id);
    }

    public static List<Integer> hotIds() {
        ensureLoaded();
        return HOT;
    }

    /** 商品详情文本里的 "占用体积,0.3立方" */
    public static double volume(Goods g) {
        String v = detailValue(g, "占用体积");
        if (v != null) {
            try {
                return Double.parseDouble(v.replace("立方", "").trim());
            } catch (Exception ignored) {}
        }
        return 0.5;
    }

    /** 商品详情文本里的 "回收价格,1灵石" */
    public static int recyclePrice(Goods g) {
        String v = detailValue(g, "回收价格");
        if (v != null) {
            try {
                return (int) Double.parseDouble(v.replace("灵石", "").trim());
            } catch (Exception ignored) {}
        }
        return 1;
    }

    private static String detailValue(Goods g, String key) {
        if (g == null || g.GoodsDetail == null) return null;
        for (String s : g.GoodsDetail) {
            if (s == null) continue;
            int c = s.indexOf(',');
            if (c < 0) c = s.indexOf('，');
            if (c < 0) continue;
            String k = s.substring(0, c).trim();
            if (k.equals(key)) return s.substring(c + 1).trim();
        }
        return null;
    }
}
