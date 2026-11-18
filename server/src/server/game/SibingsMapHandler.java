package server.game;

import xzj.Xzj;

import java.util.ArrayList;
import java.util.List;

/**
 * 1007001 兄弟地图。
 *
 * 外出历练界面要求 9~12 个地块。这里给当前城池 + 5 座邻城 + 6 片妖兽林地，
 * 共 12 个。林地点击后会走 1007013 展开成 3 个可战斗区域。
 */
public class SibingsMapHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.SibingsMapReq_1007001 req = Xzj.SibingsMapReq_1007001.parseFrom(msg.getData());
        Xzj.SibingsMapResp_1007001.Builder b = Xzj.SibingsMapResp_1007001.newBuilder();
        b.setCode(2000).setIsSuccess(true);

        MapTree.Node cur = MapTree.find(req.getMapId());
        if (cur != null) {
            List<MapTree.Node> list = new ArrayList<>();
            if (cur.type == 51) {
                // 当前城
                list.add(cur);
                // 5 座邻城
                int n = 0;
                for (MapTree.Node c : MapTree.children(cur.parentId)) {
                    if (c.id.equals(cur.id)) continue;
                    list.add(c);
                    if (++n >= 5) break;
                }
                // 6 片妖兽林地
                list.addAll(MapTree.wildsOf(cur.parentId));
            } else {
                for (MapTree.Node c : MapTree.children(cur.parentId)) {
                    if (!c.id.equals(cur.id)) list.add(c);
                }
            }
            for (MapTree.Node node : list) b.addObject(toObj(node));
        }
        session.send(1007001, msg.getMsid(), b.build());
    }

    static Xzj.SibingsMapResp_1007001.MapObjectResp_1007002.Builder toObj(MapTree.Node n) {
        Xzj.SibingsMapResp_1007001.MapObjectResp_1007002.Builder sb =
                Xzj.SibingsMapResp_1007001.MapObjectResp_1007002.newBuilder();
        sb.setId(n.id);
        sb.setParentId(n.parentId);
        MapTree.Node p = MapTree.find(n.parentId);
        sb.setParentName(p == null ? "" : p.name);
        sb.setName(n.name);
        sb.setSibingsId("");
        sb.setChildrenIds("");
        sb.setType(n.type);
        sb.setMinLevel(n.minLevel);
        sb.setMaxLevel(n.maxLevel);
        sb.setStatus(1);
        sb.setRestrict("");
        sb.setBasedConcentration(1.0);
        sb.setFiveElements(0);
        sb.setSort(1);
        sb.setLevel(n.level);
        return sb;
    }
}
