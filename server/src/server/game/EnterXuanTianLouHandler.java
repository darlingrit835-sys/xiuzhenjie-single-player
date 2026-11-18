package server.game;

import xzj.Xzj;

/**
 * 1004033 进入玄天楼：把系统价与限购次数下发给客户端。
 * 客户端只用这份数据覆盖价格、隐藏今日已买满的限购商品，所以按原价、次数 0 回即可。
 */
public class EnterXuanTianLouHandler implements MsgHandler {

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterXuanTianLouResp_1004033.Builder b = Xzj.EnterXuanTianLouResp_1004033.newBuilder();
        b.setIsSuccess(true).setCode(4295);
        for (XuanTianLouService.Goods g : XuanTianLouService.all().values()) {
            b.addGoods(Xzj.EnterXuanTianLouResp_1004033.GoodsInfoResp_1004034.newBuilder()
                    .setId(g.Id)
                    .setSystemPrice(g.GoodsPrice)
                    .setCount(0)
                    .build());
        }
        for (Integer id : XuanTianLouService.hotIds()) {
            b.addReXiaoGoodsId(id);
        }
        session.send(1004033, msg.getMsid(), b.build());
    }
}
