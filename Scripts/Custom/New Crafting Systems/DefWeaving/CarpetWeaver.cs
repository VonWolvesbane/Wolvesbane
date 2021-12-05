using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCarpetWeaver : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBCarpetWeaver()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return this.m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return this.m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
				this.Add( new GenericBuyInfo( typeof( Dyes ), 8, 20, 0xFA9, 0 ) ); 
				this.Add( new GenericBuyInfo( typeof( Wool ), 62, 20, 0xDF8, 0 ) );
				this.Add( new GenericBuyInfo( typeof( Flax ), 102, 20, 0x1A9C, 0 ) );
				this.Add( new GenericBuyInfo( typeof( WeaversSpool), 100, 20, 0x1420, 0 ) );
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
				this.Add( typeof( Flax ), 51 );
				this.Add( typeof( Wool ), 31 );
            }
        }
    }
}
