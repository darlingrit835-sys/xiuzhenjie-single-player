using System;

namespace XiuZhenJie
{
    [Serializable] internal sealed class AuctionHallState
    {
        public string eventId, tier, phase, viewTag;
        public bool unchanged;
        public long now, collectAt, previewAt, liveAt, endsAt, nextCollectAt, balance;
        public int limit, lotSeconds;
        public Lot[] lots;
        public Goods[] goods;
        public Delivery[] deliveries;
        public Service[] services;
        public Seat seat;
        public Message[] messages;

        [Serializable] internal class Goods { public string id, name, templateId, blankId; public int count, rank, kind; }
        [Serializable] internal sealed class Lot : Goods
        {
            public long price, step, start, opensAt, endsAt, myMax, lastBid;
            public bool mine, leading, participated;
            public string source, seller, leader, status;
        }
        [Serializable] internal sealed class Delivery : Goods { public string reason; }
        [Serializable] internal sealed class Service { public string id, name, effect; public long price; public int quota, cooldown; }
        [Serializable] internal sealed class Seat { public string service, room, effect; public int remaining, cooldown; public long nextAt; }
        [Serializable] internal sealed class Message { public string id, lotId, name, room, text, effect; public long at; }
        [Serializable] internal sealed class Order
        {
            public string action, tier, eventId, requestId, lotId, goodsId, service, text, viewTag;
            public long amount;
            public int count;
        }
        [Serializable] internal sealed class Failure { public string error; public bool retry; }
    }
}
