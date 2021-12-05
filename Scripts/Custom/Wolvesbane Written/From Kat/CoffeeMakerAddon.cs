/* Created by Hammerhand */

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CoffeeMakerAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new CoffeeMakerAddonDeed();
			}
		}

		[ Constructable ]
		public CoffeeMakerAddon()
		{
			AddonComponent ac;
			ac = new AddonComponent( 4323 );
			ac.Hue = 1150;
			ac.Name = "Mr.UO Coffee maker";
			AddComponent( ac, 0, 0, 0 );
            ac = new AddonComponent( 478 );
            ac.Hue = 1150;
            ac.Name = "Mr.UO Coffee maker";
            AddComponent( ac, 1, 1, 5 );
            ac = new AddonComponent( 9245 );
            ac.Hue = 1015;
            ac.Name = "CoffeePot";
            AddComponent( ac, 0, 0, 2 );
            ac = new AddonComponent( 9244 );
            ac.Hue = 1015;
            ac.Name = "Filterbasket";
            AddComponent( ac, 0, 0, 5 );
			ac = new AddonComponent( 4100 );
			ac.Name = "Handle";
			AddComponent( ac, 0, 0, 3 );

		}

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
                from.SendMessage("You are too far away.");
            else
            {
                {
                    from.SendMessage("You pour yourself a fresh, hot cup of coffee");
                    from.AddToBackpack(new HotCoffee());
                }
            }
        }
		public CoffeeMakerAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class CoffeeMakerAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new CoffeeMakerAddon();
			}
		}

		[Constructable]
		public CoffeeMakerAddonDeed()
		{
			Name = "CoffeeMaker";
		}

		public CoffeeMakerAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}