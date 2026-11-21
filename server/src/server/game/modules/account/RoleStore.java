package server.game;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import server.common.JsonStore;

import java.lang.reflect.Type;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

public class RoleStore {
    private static final Gson GSON = new Gson();
    public static final RoleStore INSTANCE = new RoleStore();
    private final JsonStore store = new JsonStore("roles");

    public synchronized Map<String, Object> getRole(String roleId) {
        store.reload();
        Object o = store.get(roleId);
        if (o == null) return null;
        Type t = new TypeToken<Map<String, Object>>() {}.getType();
        return GSON.fromJson(GSON.toJson(o), t);
    }

    public synchronized Map<String, Object> createRole(String userId, String name, boolean sex, int headPortraitId, String mapId,
                                                      double gold, double wood, double water, double fire, double earth) {
        Map<String, Object> r = new HashMap<>();
        r.put("Id", UUID.randomUUID().toString().replace("-", ""));
        r.put("UserId", userId);
        r.put("Name", name);
        r.put("Sex", sex ? 1 : 0);
        r.put("HeadPortraitId", headPortraitId);
        r.put("MapId", mapId);
        r.put("MapName", "新手村");
        r.put("Level", 1);
        r.put("CurrentAge", 16);
        r.put("MaxAge", 150);
        r.put("ShinyStone", 500L);
        r.put("ShinyJade", 100);
        r.put("PureBlood", 100);
        r.put("FlexibleRoot", "0");
        r.put("SuperAir", 100L);
        r.put("Perception", 100L);
        r.put("Physique", 100L);
        r.put("CurrentSuperAir", 100L);
        r.put("CurrentPerception", 100L);
        r.put("CurrentPhysique", 100L);
        r.put("CurrentVigor", 100);
        r.put("CurrentClever", 100);
        r.put("CurrentPoisonVal", 0);
        r.put("CurrentPureBlood", 100);

        r.put("Gold", gold);
        r.put("Wood", wood);
        r.put("Water", water);
        r.put("Fire", fire);
        r.put("Earth", earth);

        java.util.List<Integer> lingGen = new java.util.ArrayList<>();
        if (gold > 0) lingGen.add(1);
        if (wood > 0) lingGen.add(2);
        if (water > 0) lingGen.add(3);
        if (fire > 0) lingGen.add(4);
        if (earth > 0) lingGen.add(5);
        r.put("HasGold", gold > 0);
        r.put("HasWood", wood > 0);
        r.put("HasWater", water > 0);
        r.put("HasFire", fire > 0);
        r.put("HasEarth", earth > 0);
        for (int i = 0; i < 4; i++) {
            r.put("Live" + (i + 1) + "Type", i < lingGen.size() ? lingGen.get(i) : 0);
            r.put("Live" + (i + 1) + "Level", i < lingGen.size() ? 1 : 0);
            r.put("Live" + (i + 1) + "Value", 0);
        }
        r.put("LiveRelId", 0);
        r.put("YueKaDays", 0);
        r.put("Title", 0);
        r.put("BattleGuideVal", 1);
        r.put("PracticeGuideVal", 1);
        r.put("IsNewLetter", false);
        r.put("AcupointGuideVal", 1);
        r.put("ShopGuideVal", 1);
        r.put("HunLiVal", 0);
        r.put("SignGuideVal", 1);
        r.put("ElixirGuideVal", 1);
        r.put("IsGetSignIn", false);
        r.put("SignInDay", 0);
        r.put("TaskId", 1);
        r.put("TaskStatus", 0);
        RoleUtil.initNewRole(r);
        saveWithAcupoints(r);
        return r;
    }

    public synchronized boolean nameExists(String name) {
        for (Map.Entry<String, Object> e : store.all().entrySet()) {
            Type t = new TypeToken<Map<String, Object>>() {}.getType();
            Map<String, Object> r = GSON.fromJson(GSON.toJson(e.getValue()), t);
            if (name.equals(r.get("Name"))) return true;
        }
        return false;
    }

    private static final String[] ACUPOINT_FIELDS = {"AcupointValue", "Acupoints"};

    static void copyAcupoints(Map<String, Object> source, Map<String, Object> target) {
        for (String field : ACUPOINT_FIELDS) {
            if (source.containsKey(field)) target.put(field, source.get(field));
            else target.remove(field);
        }
    }

    @SuppressWarnings("unchecked")
    public synchronized void save(Map<String, Object> r) {
        Object current = store.get((String) r.get("Id"));
        if (current instanceof Map) copyAcupoints((Map<String, Object>) current, r);
        saveWithAcupoints(r);
    }

    synchronized void saveWithAcupoints(Map<String, Object> r) {
        store.put((String) r.get("Id"), new HashMap<>(r));
    }

    @SuppressWarnings("unchecked")
    synchronized void savePromotion(Map<String, Object> r, int previousLevel, int award) {
        Object current = store.get((String) r.get("Id"));
        Map<String, Object> basis;
        if (current instanceof Map) {
            basis = (Map<String, Object>) current;
            copyAcupoints(basis, r);
        } else {
            basis = new HashMap<>(r);
            basis.put("Level", previousLevel);
        }
        if (RoleUtil.num(basis, "Level", 1) < RoleUtil.num(r, "Level")) {
            long balance = Math.max(0, Math.min(Integer.MAX_VALUE, Meridians.balance(basis)));
            r.put("AcupointValue", (int) Math.min(Integer.MAX_VALUE, balance + Math.max(0, award)));
        }
        saveWithAcupoints(r);
    }
}
