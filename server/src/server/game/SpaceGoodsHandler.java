package server.game;

import xzj.Xzj;

import java.util.List;
import java.util.Map;

/**
 * 1004002 纳戒物品列表。客户端打开背包/装备界面时都会走这里。
 */
public class SpaceGoodsHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.SpaceGoodsReq_1004002 req = Xzj.SpaceGoodsReq_1004002.parseFrom(msg.getData());
        Xzj.SpaceGoodsResp_1004002.Builder b = Xzj.SpaceGoodsResp_1004002.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            GoodsService.ensureStarterKit(session.role);
            int location = req.getLocation();
            b.setIsSuccess(true).setCode(2000).setLocation(location);
            if (location == GoodsService.LOCATION_NAJIE || location == 0) {
                List<Map<String, Object>> list = GoodsService.all(session.role);
                for (Map<String, Object> g : list) b.addGoodsInfos(toProto(g));
            }
            b.setNowCapacity(GoodsService.capacity(session.role));
            b.setMaxCapacity(GoodsService.MAX_CAPACITY);
            RoleUtil.save(session.role);
        }
        session.send(1004002, msg.getMsid(), b.build());
    }

    static Xzj.SpaceGoodsResp_1004002.GoodsInfoResp_1004003 toProto(Map<String, Object> g) {
        Xzj.SpaceGoodsResp_1004002.GoodsInfoResp_1004003.Builder o =
                Xzj.SpaceGoodsResp_1004002.GoodsInfoResp_1004003.newBuilder();
        o.setGoodsId(GoodsService.str(g, "GoodsId"));
        o.setName(GoodsService.str(g, "Name"));
        o.setFiveElement(GoodsService.num(g, "FiveElement", 1));
        o.setRank(GoodsService.num(g, "Rank", 1));
        o.setLevel(GoodsService.num(g, "Level", 1));
        o.setType(GoodsService.num(g, "Type", 0));
        o.setIsUsing(GoodsService.bool(g, "IsUsing"));
        o.setIsBinding(GoodsService.bool(g, "IsBinding"));
        o.setRecyclePrice(GoodsService.num(g, "RecyclePrice", 0));
        o.setTemplateId(GoodsService.str(g, "TemplateId"));
        o.setConversionRate(GoodsService.num(g, "ConversionRate", 1));
        o.setVolume(GoodsService.num(g, "Volume", 1));
        o.setIsLock(GoodsService.bool(g, "IsLock"));
        o.setCount(Math.max(1, GoodsService.num(g, "Count", 1)));
        return o.build();
    }
}
