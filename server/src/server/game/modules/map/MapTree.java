package server.game;

import java.util.ArrayList;
import java.util.List;

public class MapTree {
    public static class Node {
        public String id;
        public String parentId;
        public String name;
        public int type;
        public int level;
        public int minLevel;
        public int maxLevel;

        public Node(String id, String parentId, String name, int type, int level, int minLevel, int maxLevel) {
            this.id = id;
            this.parentId = parentId;
            this.name = name;
            this.type = type;
            this.level = level;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
        }
    }

    private static final List<Node> TREE = new ArrayList<>();

    private static void city(String junId, int idx, String name) {
        int min = idx * 10 - 9;
        int max = idx * 10;
        String cityId = junId + String.format("%02d", idx);
        TREE.add(new Node(cityId, junId, name, 51, 4, min, max));
        TREE.add(new Node(cityId + "01", cityId, name + "·城主府", 4, 5, min, max));
        TREE.add(new Node(cityId + "02", cityId, name + "·玄天楼", 4, 5, min, max));
        TREE.add(new Node(cityId + "03", cityId, name + "·虚神界", 4, 5, min, max));
        TREE.add(new Node(cityId + "04", cityId, name + "·中阶坊市", 4, 5, min, max));
        TREE.add(new Node(cityId + "05", cityId, name + "·传送大阵", 4, 5, min, max));
        TREE.add(new Node(cityId + "06", cityId, name + "·灵泉酒肆", 4, 5, min, max));
    }

    private static void jun(String zhouId, String junId, String junName, String... cities) {
        TREE.add(new Node(junId, zhouId, junName, 3, 3, 1, 40));
        for (int i = 0; i < cities.length; i++) {
            city(junId, i + 1, cities[i]);
        }
    }

    static {
        TREE.add(new Node("1", "0", "修真界", 0, 0, 0, 999));
        TREE.add(new Node("11", "1", "东荒", 1, 1, 0, 999));
        TREE.add(new Node("12", "1", "南岭", 1, 1, 0, 999));
        TREE.add(new Node("13", "1", "西漠", 1, 1, 0, 999));
        TREE.add(new Node("14", "1", "北冥", 1, 1, 0, 999));

        TREE.add(new Node("111", "11", "青云洲", 2, 2, 0, 40));
        TREE.add(new Node("112", "11", "落日洲", 2, 2, 0, 40));
        TREE.add(new Node("121", "12", "紫霞洲", 2, 2, 0, 40));
        TREE.add(new Node("131", "13", "黄沙洲", 2, 2, 0, 40));
        TREE.add(new Node("141", "14", "玄冰洲", 2, 2, 0, 40));

        jun("111", "1111", "临仙郡",
                "新手村", "临仙城", "翠微城", "听雨城", "落霞城", "望月城",
                "飞星城", "烟波城", "问剑城", "青冥城", "紫电城", "浮光城");
        jun("111", "1112", "朝露郡",
                "朝露城", "晨光城", "清溪城", "白鹭城", "松风城", "竹影城",
                "梅香城", "兰溪城", "菊隐城", "荷风城", "桂月城", "雪梅城");
        jun("112", "1121", "云梦郡",
                "云梦城", "楚云城", "碧涛城", "沧浪城", "凌波城", "归雁城",
                "栖霞城", "映月城", "回风城", "断虹城", "流云城", "惊鸿城");
        jun("121", "1211", "紫霞郡",
                "紫霞城", "丹霞城", "赤霄城", "朱明城", "绛云城", "彤光城",
                "瑞彩城", "宝华城", "流金城", "鎏光城", "琉璃城", "琥珀城");
        jun("131", "1311", "沙海郡",
                "沙海城", "绿洲城", "鸣沙城", "断魂城", "孤烟城", "落日城",
                "瀚海城", "苍狼城", "古楼城", "驼铃城", "金沙城", "月牙城");
        jun("141", "1411", "寒渊郡",
                "寒渊城", "冰魄城", "雪岭城", "霜华城", "凝碧城", "玉衡城",
                "太清城", "广寒城", "玄冥城", "寒星城", "朔风城", "琼华城");
    }

    public static Node find(String id) {
        for (Node n : TREE) {
            if (n.id.equals(id)) return n;
        }
        // 妖兽林地 / 林地子区域是动态生成的，不放进郡的子节点里，避免破坏 12 城面板
        if (id != null && id.contains("W")) {
            int w = id.indexOf('W');
            String junId = id.substring(0, w);
            Node jun = find(junId);
            if (jun != null) {
                String rest = id.substring(w + 1);
                if (rest.length() == 1) {
                    int idx = rest.charAt(0) - '0';
                    if (idx >= 1 && idx <= 6) return wild(jun, idx);
                } else if (rest.length() == 2) {
                    int idx = rest.charAt(0) - '0';
                    int sub = rest.charAt(1) - '0';
                    if (idx >= 1 && idx <= 6 && sub >= 1 && sub <= 3) {
                        Node parent = wild(jun, idx);
                        return wildChild(parent, sub);
                    }
                }
            }
        }
        return null;
    }

    public static Node wild(Node jun, int idx) {
        return new Node(jun.id + "W" + idx, jun.id, jun.name + "·妖兽林地" + idx, 4, 4, 1, 999);
    }

    public static Node wildChild(Node parent, int sub) {
        String[] names = {"外围", "深处", "兽王巢"};
        return new Node(parent.id + sub, parent.id, parent.name + "·" + names[Math.max(0, Math.min(2, sub - 1))],
                4, 5, 1, 999);
    }

    /** 某个郡的 6 片妖兽林地。 */
    public static List<Node> wildsOf(String junId) {
        List<Node> out = new ArrayList<>();
        Node jun = find(junId);
        if (jun == null) return out;
        for (int i = 1; i <= 6; i++) out.add(wild(jun, i));
        return out;
    }

    /** 一片林地的三个子区域。 */
    public static List<Node> wildChildren(String wildId) {
        List<Node> out = new ArrayList<>();
        Node w = find(wildId);
        if (w == null) return out;
        for (int i = 1; i <= 3; i++) out.add(wildChild(w, i));
        return out;
    }

    public static List<Node> children(String parentId) {
        List<Node> out = new ArrayList<>();
        for (Node n : TREE) {
            if (n.parentId.equals(parentId)) out.add(n);
        }
        return out;
    }

    public static Node ancestor() {
        return find("1");
    }
}
