package server.game;

import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.*;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.handler.codec.protobuf.ProtobufDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import server.common.Config;
import server.common.ServerConfig;
import xzj.Xzj;

import java.util.Map;

/**
 * 独立战斗服。客户端在 BuildDungeon 后会连接到这里。
 *
 * 战斗过程由客户端本地演算，服务端只负责：
 *  - 下发怪物与地图信息（8001001）
 *  - 放行技能（8001005）
 *  - 接收怪物死亡（8002003）并下发掉落
 *  - 离开副本（8001008）
 */
public class BattleServer {
    private EventLoopGroup boss;
    private EventLoopGroup worker;

    public void start() {
        int port = ServerConfig.get().battlePort;
        boss = new NioEventLoopGroup(1);
        worker = new NioEventLoopGroup();
        try {
            ServerBootstrap b = new ServerBootstrap();
            b.group(boss, worker)
                    .channel(NioServerSocketChannel.class)
                    .childOption(ChannelOption.TCP_NODELAY, true)
                    .childHandler(new ChannelInitializer<SocketChannel>() {
                        @Override
                        protected void initChannel(SocketChannel ch) {
                            ch.pipeline()
                                    .addLast(new ProtobufVarint32FrameDecoder())
                                    .addLast(new ProtobufDecoder(Xzj.Msg.getDefaultInstance()))
                                    .addLast(new Handler());
                        }
                    });
            java.net.InetSocketAddress address = Config.listenAddress(port);
            ChannelFuture f = b.bind(address).sync();
            System.out.println("[BattleServer] listening on tcp://" + address.getHostString() + ":" + address.getPort());
            f.channel().closeFuture().sync();
        } catch (Exception e) {
            throw new IllegalStateException("[BattleServer] failed on port " + port, e);
        } finally {
            boss.shutdownGracefully();
            worker.shutdownGracefully();
        }
    }

    private static class Handler extends SimpleChannelInboundHandler<Xzj.Msg> {
        private String dungeonId;
        private String roleId;

        @Override
        public void channelActive(ChannelHandlerContext ctx) {
            System.out.println("[BattleServer] connected: " + ctx.channel().remoteAddress());
        }

        @Override
        public void channelInactive(ChannelHandlerContext ctx) {
            System.out.println("[BattleServer] disconnected: " + ctx.channel().remoteAddress());
        }

        @Override
        protected void channelRead0(ChannelHandlerContext ctx, Xzj.Msg msg) {
            try {
                switch (msg.getCmd()) {
                    case 8001001:
                        onEnter(ctx, msg);
                        break;
                    case 8001005:
                        onUseSkill(ctx, msg);
                        break;
                    case 8001006:
                        // 伤害由客户端演算，服务端不需要回包
                        break;
                    case 8001004:
                    case 8001014:
                    case 9000000:
                        // 移动/受伤/心跳：高频消息，静默处理，避免刷日志
                        break;
                    case 8002003:
                        onMonsterDead(ctx, msg);
                        break;
                    case 8001007:
                        onPickGoods(ctx, msg);
                        break;
                    case 8001008:
                        onLeave(ctx, msg);
                        break;
                    default:
                        System.out.println("[BattleServer] cmd=" + msg.getCmd() + " ignored");
                }
            } catch (Exception e) {
                System.err.println("[BattleServer] handler error cmd=" + msg.getCmd() + ": " + e);
                e.printStackTrace();
            }
        }

        private void onEnter(ChannelHandlerContext ctx, Xzj.Msg msg) throws Exception {
            Xzj.BattleRoleEnterReq_8001001 req = Xzj.BattleRoleEnterReq_8001001.parseFrom(msg.getData());
            dungeonId = req.getDungeonId();
            roleId = req.getRoleId();
            Dungeon d = DungeonManager.get(dungeonId);
            if (d == null) d = DungeonManager.findAnyForRole(roleId);
            if (d == null && roleId != null) {
                Map<String, Object> role = RoleStore.INSTANCE.getRole(roleId);
                if (role != null) {
                    RoleUtil.ensureDefaults(role);
                    d = DungeonManager.create(role, "1");
                }
            }
            Xzj.BattleRoleEnterResp_8001001.Builder b = Xzj.BattleRoleEnterResp_8001001.newBuilder();
            if (d == null) {
                b.setIsSuccess(false).setCode(4503);
                send(ctx, 8001001, msg.getMsid(), b.build());
                return;
            }
            dungeonId = d.id;
            b.setIsSuccess(true).setCode(4506);
            b.setMapMinX(-10).setMapMaxX(10).setMapMinY(-2).setMapMaxY(7);
            b.setPosX(0).setPosY(-2).setAngle(90);
            b.setRoleIndex(d.roleIndex);
            b.setOverTime(Math.max(60000, ServerConfig.get().battleDurationSeconds * 1000L));

            for (Dungeon.Monster m : d.monsters) {
                Xzj.BattleRoleEnterResp_8001001.ObjectInfoResp_8001002.Builder o =
                        Xzj.BattleRoleEnterResp_8001001.ObjectInfoResp_8001002.newBuilder();
                o.setObjectIndex(m.index);
                o.setPosX(m.x).setPosY(m.y).setAngle(0);
                o.setFiveElement(m.fiveElement);
                o.setObjectType(m.objectType);
                o.setName(m.name);
                o.setLevel(m.level);
                o.setCurrentPhysiqueDefense((int) Math.min(Integer.MAX_VALUE, m.armor));
                o.setCurrentPureBloodDefense((int) Math.min(Integer.MAX_VALUE, m.hp));
                o.setCurrentSuperAir((int) Math.min(Integer.MAX_VALUE, m.mana));
                o.setCurrentPerception(m.perception);
                o.setCurrentMoveSpeed(m.speed);
                o.setIcon(m.icon);
                o.setSuperAirRecoverValue(m.mana * 0.01);
                o.setDefenseRecoverValue(m.armor * 0.01);
                o.setSuperAirRecoverValueOnBattle(m.mana * 0.005);
                o.setDefenseRecoverValueOnBattle(m.armor * 0.005);
                o.setSuperAirPower(m.attack);
                o.setAttackDistance(5.0);
                o.setUnusualLevel(m.unusual);
                for (int st : monsterSkillTypes(m)) {
                    o.addSkills(monsterSkill(m, st));
                }
                b.addObjects(o);
            }
            send(ctx, 8001001, msg.getMsid(), b.build());
        }

        private int[] monsterSkillTypes(Dungeon.Monster m) {
            if (m.unusual == 4) return new int[]{1, 4, 2, 3, 5, 8};
            if (m.unusual == 3) return new int[]{1, 4, 2, 7};
            return new int[]{1, 2};
        }

        private Xzj.BattleRoleEnterResp_8001001.ObjectInfoResp_8001002.ObjectInfoSkillResp_8001003.Builder
        monsterSkill(Dungeon.Monster m, int skillType) {
            Xzj.BattleRoleEnterResp_8001001.ObjectInfoResp_8001002.ObjectInfoSkillResp_8001003.Builder s =
                    Xzj.BattleRoleEnterResp_8001001.ObjectInfoResp_8001002.ObjectInfoSkillResp_8001003.newBuilder();
            double mult;
            int cd, cast, cont;
            String name;
            switch (skillType) {
                case 2:  // 噬咬：持续伤害
                    mult = 0.55; cd = 8; cast = 300; cont = 5000; name = "噬咬";
                    break;
                case 3:  // 冲锋：突进/控制
                    mult = 0.45; cd = 10; cast = 400; cont = 0; name = "冲锋";
                    break;
                case 4:  // 妖术：远程法术
                    mult = 1.15; cd = 6; cast = 800; cont = 0; name = "妖术";
                    break;
                case 5:  // 厚皮：防御增益
                    mult = 0.10; cd = 15; cast = 500; cont = 8000; name = "厚皮";
                    break;
                case 6:  // 吞噬：减伤
                    mult = 0.10; cd = 15; cast = 500; cont = 8000; name = "吞噬";
                    break;
                case 7:  // 束缚：减速
                    mult = 0.25; cd = 10; cast = 400; cont = 0; name = "束缚";
                    break;
                case 8:  // 震慑：延迟施法
                    mult = 0.25; cd = 12; cast = 500; cont = 0; name = "震慑";
                    break;
                default: // 普杀
                    mult = 1.0; cd = m.unusual == 4 ? 2 : 3; cast = 300; cont = 0; name = "扑咬";
                    break;
            }
            s.setIcon(skillType);
            s.setType(skillType);
            s.setSkillType(1);
            s.setName(name);
            s.setTemplateId("monster_skill_" + skillType);
            s.setCastTime(cast);
            s.setContinueTime(cont);
            s.setCoolDown(cd);
            s.setFiveElement(m.fiveElement);
            s.setConversionRate(1.0);
            s.setBaseDamage(Math.max(1, Math.round(m.attack * mult)));
            s.setUsedPerception(0);
            s.setLevel(m.level);
            s.setSkillIndex(skillType);
            s.setUsedSuperAir(0);
            s.setCanUseCount(9999);
            s.setRank(m.unusual);
            s.setUnitUsed(0);
            s.setGoldDamageAdd(1.0);
            s.setWoodDamageAdd(1.0);
            s.setWaterDamageAdd(1.0);
            s.setFireDamageAdd(1.0);
            s.setEarthDamageAdd(1.0);
            return s;
        }

        private void onUseSkill(ChannelHandlerContext ctx, Xzj.Msg msg) throws Exception {
            Xzj.BattleRoleUseSkillResp_8001005.Builder b = Xzj.BattleRoleUseSkillResp_8001005.newBuilder();
            b.setIsSuccess(true).setCode(4534);
            send(ctx, 8001005, msg.getMsid(), b.build());
        }

        private void onMonsterDead(ChannelHandlerContext ctx, Xzj.Msg msg) throws Exception {
            Xzj.MonsterDeadReq_8002003 req = Xzj.MonsterDeadReq_8002003.parseFrom(msg.getData());
            Dungeon d = DungeonManager.get(dungeonId);
            if (d == null) return;
            Dungeon.Monster hit = null;
            for (Dungeon.Monster m : d.monsters) {
                if (m.index == req.getDeadMonsterIndex()) { hit = m; break; }
            }
            if (hit == null) return;
            if (!d.dead.add(hit.index)) return;

            int count = (int) Math.max(1, Math.min(Integer.MAX_VALUE, hit.dropGold));
            Xzj.MonsterDeadResp_8002003.Builder b = Xzj.MonsterDeadResp_8002003.newBuilder();
            b.setDeadMonsterIndex(hit.index);
            b.setGoodsId("lingshi_" + count);
            b.setPosX(req.getPosX());
            b.setPosY(req.getPosY());
            b.setFiveElement(hit.fiveElement);
            b.setGoodsType(100);       // 100 -> 灵石掉落
            b.setName("灵石");
            b.setLevel(hit.level);
            b.setRank(1);
            b.setObjectIndex(hit.index);
            b.setCount(count);
            send(ctx, 8002003, 0, b.build());

            // 战斗奖励：修为、任务击杀、物品掉落
            BattleRewardService.Result res = BattleRewardService.onKill(roleId, hit);
            for (BattleRewardService.Drop drop : res.drops) {
                Xzj.MonsterDeadResp_8002003.Builder db = Xzj.MonsterDeadResp_8002003.newBuilder();
                db.setDeadMonsterIndex(hit.index);
                db.setGoodsId(drop.dropId);
                db.setPosX(req.getPosX() + 0.6);
                db.setPosY(req.getPosY() + 0.4);
                db.setFiveElement(drop.fiveElement);
                db.setGoodsType(drop.type);
                db.setName(drop.name);
                db.setLevel(drop.level);
                db.setRank(drop.rank);
                db.setObjectIndex(hit.index);
                db.setCount(drop.count);
                send(ctx, 8002003, 0, db.build());
            }

            // 任务：完成一次扫荡（整波怪清空）
            if (d.allDead() && !d.sweepCounted) {
                d.sweepCounted = true;
                BattleRewardService.Result sweep = BattleRewardService.onSweep(roleId, d.playerLevel);
                for (BattleRewardService.Drop drop : sweep.drops) {
                    Xzj.MonsterDeadResp_8002003.Builder db = Xzj.MonsterDeadResp_8002003.newBuilder();
                    db.setDeadMonsterIndex(hit.index);
                    db.setGoodsId(drop.dropId);
                    db.setPosX(req.getPosX());
                    db.setPosY(req.getPosY());
                    db.setFiveElement(drop.fiveElement);
                    db.setGoodsType(drop.type);
                    db.setName(drop.name);
                    db.setLevel(drop.level);
                    db.setRank(drop.rank);
                    db.setObjectIndex(hit.index);
                    db.setCount(drop.count);
                    send(ctx, 8002003, 0, db.build());
                }
            }
        }

        private void onPickGoods(ChannelHandlerContext ctx, Xzj.Msg msg) throws Exception {
            Xzj.RolePickGoodsResp_8001007.Builder b = Xzj.RolePickGoodsResp_8001007.newBuilder();
            b.setIsSuccess(true).setCode(4526);
            send(ctx, 8001007, msg.getMsid(), b.build());
        }

        private void onLeave(ChannelHandlerContext ctx, Xzj.Msg msg) throws Exception {
            Xzj.RoleLeaveDungeonResp_8001008.Builder b = Xzj.RoleLeaveDungeonResp_8001008.newBuilder();
            b.setIsSuccess(true).setCode(4537);
            send(ctx, 8001008, msg.getMsid(), b.build());
            if (roleId != null) BattleRewardService.clearLoot(roleId);
            if (dungeonId != null) DungeonManager.remove(dungeonId);
        }

        private void send(ChannelHandlerContext ctx, int cmd, int msid, com.google.protobuf.MessageLite payload) {
            Xzj.Msg.Builder mb = Xzj.Msg.newBuilder();
            mb.setCmd(cmd);
            if (msid > 0) mb.setMsid(msid);
            if (payload != null) mb.setData(payload.toByteString());
            RawSender.send(ctx.channel(), mb.build());
        }
    }
}
