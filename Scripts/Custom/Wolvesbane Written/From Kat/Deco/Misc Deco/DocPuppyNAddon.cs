/// Doc - Puppy S. Addon for The Playground UO  
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DocPuppySAddon : BaseAddon
	{
         
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new DocPuppySAddonDeed();
			}
		}

		[ Constructable ]
		public DocPuppySAddon()
		{



			AddComplexComponent( (BaseAddon) this, 25576, 0, 0, 0, 0, -1, "Awww a puppy!", 1);// 1

		}

		public DocPuppySAddon( Serial serial ) : base( serial )
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
                                from.Say("Pookie speak!");//
                                from.PlaySound(0x087);
                                break;
                            }
                        case 1:
                            {
                                from.Say("Is Pookie tired??"); //good
                                from.PlaySound(0x089);
                                break;
                            }

                        case 2:
                            {
                                from.Say("Sick'em Pookie!"); // wr
                                from.PlaySound(0x0E7);
                                break;
                            }

                        case 3:
                            {
                                from.Say("Does Pookie like cats?");//good
                                from.PlaySound(0x0E7); //0x089 whine
                                break;
                            }
                        case 4:
                            {
                                from.Say("Pookie.. Sing!");
                                from.PlaySound(0x0E6);
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

	public class DocPuppySAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new DocPuppySAddon();
			}
		}

		[Constructable]
		public DocPuppySAddonDeed()
		{
			Name = "DocPuppyS";
		}

		public DocPuppySAddonDeed( Serial serial ) : base( serial )
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