/// Doc - Stuffed Bear S Addon for The Playground UO  from.PlaySound( 0x086 );
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DocStuffBearS : BaseAddon
	{
         
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new DocStuffBearSDeed();
			}
		}

		[ Constructable ]
		public DocStuffBearS()
		{



			AddComplexComponent( (BaseAddon) this,0x9A26, 0, 0, 0, 0, -1, "Awww a cuddly bear!", 1);// 1

		}

		public DocStuffBearS( Serial serial ) : base( serial )
		{
		}

		public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
                from.SendMessage("You are too far away.");
            else
            {
            
                    switch (Utility.Random(5))
                    {
                        default:
                      case 0:
                            {
                                from.Say("Who's a good wittle bear?");//
                                from.PlaySound(0x0A4);
                                break;
                            }
                        case 1:
                            {
                                from.Say("Are you hungry??"); //good
                                from.PlaySound(0x0A5);
                                break;
                            }

                        case 2:
                            {
                                from.Say("Sick'em!"); // wr
                                from.PlaySound(0x0A6);
                                break;
                            }

                        case 3:
                            {
                                from.Say("Awww arent you precious?");//good
                                from.PlaySound(0x0A5); //0x089 whine
                                break;
                            }
                        case 4:
                            {
                                from.Say("Sing!");
                                from.PlaySound(0x0A4);
                                break;
                            }
                    }
                }
            }
        
		
        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
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

	public class DocStuffBearSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new DocStuffBearS();
			}
		}

		[Constructable]
		public DocStuffBearSDeed()
		{
			Name = "Stuffed Bear South";
		}

		public DocStuffBearSDeed( Serial serial ) : base( serial )
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