using System;
using System.Collections.Generic;
using Server.Engines.Apiculture;
using Server.Items;

namespace Server.Mobiles
{
    public class SBBeekeeper : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBBeekeeper()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(JarHoney), 300, 20, 0x9EC, 0));
                Add(new GenericBuyInfo(typeof(Beeswax), 200, 20, 0x1422, 0));
                Add(new GenericBuyInfo(typeof(apiBeeHiveDeed), 5000, 10, 0x14F0, 0));
                Add(new GenericBuyInfo(typeof(HiveTool), 1000, 20, 0x9F5, 0));
                Add(new GenericBuyInfo(typeof(apiSmallWaxPot), 2500, 20, 0x9E4, 0));
                Add(new GenericBuyInfo(typeof(apiLargeWaxPot), 5000, 20, 0x9ED, 0));
                Add(new GenericBuyInfo(typeof(WaxCraftingPot), 1000, 20, 0x142A, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(JarHoney), 150);
                Add(typeof(Beeswax), 100);
            }
        }
    }
}
