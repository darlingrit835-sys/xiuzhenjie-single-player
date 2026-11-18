package server.game;

import xzj.Xzj;

import java.util.Map;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

/**
 * 修炼心跳：每 2 秒（一个周天）给在线玩家结算修为/神识/体魄，并推送 9001xxx 通知。
 */
public final class CultivationService {
    private static final int TICK_SECONDS = 2;
    private static ScheduledExecutorService exec;
    private static long tick = 0;

    private CultivationService() {}

    public static synchronized void start() {
        if (exec != null) return;
        exec = Executors.newSingleThreadScheduledExecutor(r -> {
            Thread t = new Thread(r, "cultivation");
            t.setDaemon(true);
            return t;
        });
        exec.scheduleAtFixedRate(CultivationService::onTick, TICK_SECONDS, TICK_SECONDS, TimeUnit.SECONDS);
        System.out.println("[Cultivation] started, one zhou-tian = " + TICK_SECONDS + "s");
    }

    private static void onTick() {
        tick++;
        for (PlayerSession s : GameServer.SESSIONS.values()) {
            if (s == null || s.roleId == null || s.role.isEmpty() || !s.channel.isActive()) continue;
            try {
                tickSession(s);
                if (QuestService.tick(s)) {
                    GameServer.notifyTaskComplete(s);
                }
            } catch (Exception e) {
                System.err.println("[Cultivation] tick error role=" + s.roleId + ": " + e);
            }
        }
    }

    private static void tickSession(PlayerSession s) {
        Xzj.SuperAirUpResp_9001004.Builder superAir = null;
        Xzj.PhysiqueUpResp_9001005.Builder physique = null;
        Xzj.PerceptionUpResp_9001006.Builder perception = null;
        boolean doSave = false;

        synchronized (s.role) {
            Map<String, Object> r = s.role;
            RoleUtil.ensureDefaults(r);
            int lv = RoleUtil.num(r, "Level", 1);
            GameData.LevelInfo li = GameData.level(lv);
            int mask = RoleUtil.activeMask(r);
            boolean naqi = (mask & 4) != 0;
            boolean duanti = (mask & 2) != 0;
            boolean lianshen = (mask & 1) != 0;

            // 修为
            if (naqi) {
                double rate = RoleUtil.rate(r, RoleUtil.TYPE_NAQI);
                s.carryNaQi += rate * TICK_SECONDS;
                long gain = (long) Math.floor(s.carryNaQi);
                if (gain > 0) s.carryNaQi -= gain;
                long cur = RoleUtil.lng(r, "SuperAir");
                long max = (long) li.upgradeLimit;
                boolean wasBreak = cur >= max;
                if (gain > 0) {
                    long next = Math.min(max, cur + gain);
                    if (next != cur) r.put("SuperAir", next);
                    cur = next;
                }
                boolean isBreak = cur >= max;
                // 蓝量保持满，避免客户端显示“真元不足”
                long mana = Math.max(1, RoleUtil.lng(r, "Perception") * 2);
                r.put("CurrentSuperAir", mana);
                if (gain > 0 || tick % 5 == 0 || (!wasBreak && isBreak)) {
                    superAir = Xzj.SuperAirUpResp_9001004.newBuilder();
                    superAir.setCurrentSuperAir(mana);
                    superAir.setSuperAir(cur);
                    superAir.setIsBreak(!wasBreak && isBreak);
                    superAir.setSuperAirRefiningValue(0);
                    superAir.setCurrentRunValue(RoleUtil.circle(r, RoleUtil.TYPE_NAQI));
                }
                if (!wasBreak && isBreak) doSave = true;
            }

            // 体魄/精血
            if (duanti) {
                double rate = RoleUtil.rate(r, RoleUtil.TYPE_DUANTI);
                s.carryDuanTi += rate * TICK_SECONDS * 0.5;
                long gain = (long) Math.floor(s.carryDuanTi);
                if (gain > 0) s.carryDuanTi -= gain;
                long cap = (long) (li.limitToughTenacity * 1.5);
                long cur = RoleUtil.lng(r, "Physique");
                if (gain > 0 && cur < cap) {
                    long next = Math.min(cap, cur + gain);
                    r.put("Physique", next);
                    r.put("CurrentPhysique", next);
                    long pure = Math.max(RoleUtil.num(r, "PureBlood"),
                            Math.max(100, (int) Math.round(next * 0.01)));
                    r.put("PureBlood", pure);
                    r.put("CurrentPureBlood", pure);
                }
                if (gain > 0 || tick % 5 == 0) {
                    physique = Xzj.PhysiqueUpResp_9001005.newBuilder();
                    physique.setCurrentPhysique(RoleUtil.lng(r, "CurrentPhysique"));
                    physique.setPhysique(RoleUtil.lng(r, "Physique"));
                    physique.setCurrentPureBlood(RoleUtil.num(r, "CurrentPureBlood"));
                    physique.setPureBlood(RoleUtil.num(r, "PureBlood"));
                    physique.setPhysiqueRefiningValue(0);
                    physique.setCurrentRunValue(RoleUtil.circle(r, RoleUtil.TYPE_DUANTI));
                }
            }

            // 神识
            if (lianshen) {
                double rate = RoleUtil.rate(r, RoleUtil.TYPE_LIANSHEN);
                s.carryLianShen += rate * TICK_SECONDS * 0.5;
                long gain = (long) Math.floor(s.carryLianShen);
                if (gain > 0) s.carryLianShen -= gain;
                long cap = (long) (li.limitPerception * 1.5);
                long cur = RoleUtil.lng(r, "Perception");
                if (gain > 0 && cur < cap) {
                    long next = Math.min(cap, cur + gain);
                    r.put("Perception", next);
                    r.put("CurrentPerception", next);
                }
                if (gain > 0 || tick % 5 == 0) {
                    perception = Xzj.PerceptionUpResp_9001006.newBuilder();
                    perception.setCurrentPerception(RoleUtil.lng(r, "CurrentPerception"));
                    perception.setPerception(RoleUtil.lng(r, "Perception"));
                    perception.setPerceptionRefiningValue(0);
                    perception.setCurrentRunValue(RoleUtil.circle(r, RoleUtil.TYPE_LIANSHEN));
                }
            }

            if (tick % 15 == 0) doSave = true;
            if (doSave) RoleUtil.recomputePower(r);
        }

        if (doSave) RoleUtil.save(s.role);
        if (superAir != null) s.send(9001004, 0, superAir.build());
        if (physique != null) s.send(9001005, 0, physique.build());
        if (perception != null) s.send(9001006, 0, perception.build());
    }
}
