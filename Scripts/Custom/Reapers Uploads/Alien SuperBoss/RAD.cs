using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{

	public class RAD : Item
	{

		[Constructable]
		public RAD() : this( null )
		{
		}

		[Constructable]
		public RAD ( string name ) : base ( 18094 )
		{
			Name = "Weird Alien Scroll";
			Hue = 2816;
		}

		public RAD ( Serial serial ) : base ( serial )
		{
		}

      		public override void OnDoubleClick( Mobile from ) 
      		{
			if ( !IsChildOf( from.Backpack ) )
			{
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                switch (Utility.Random(14))
                {
                    case 0: from.AddToBackpack(new ArtifactArm()); break;
                    case 1: from.AddToBackpack(new ArtifactChest()); break;
                    case 2: from.AddToBackpack(new ArtifactGlove()); break;
                    case 3: from.AddToBackpack(new ArtifactHelm()); break;
                    case 4: from.AddToBackpack(new ArtifactLegging()); break;
                    case 5: from.AddToBackpack(new ArtifactNeck()); break;
                    case 6: from.AddToBackpack(new ArtifactCape()); break;
                    case 7: from.AddToBackpack(new ArtifactHalfApron()); break;
                    case 8: from.AddToBackpack(new ArtifactRobe()); break;
                    case 9: from.AddToBackpack(new ArtifactShoes()); break;
                    case 10: from.AddToBackpack(new ArtifactBracelet()); break;
                    case 11: from.AddToBackpack(new ArtifactEarring()); break;
                    case 12: from.AddToBackpack(new ArtifactRing()); break;
                    case 13: from.AddToBackpack(new ArtifactShield()); break;
               }
			    Effects.PlaySound(from.Location, from.Map, 0x1F7);
				Effects.SendTargetParticles(from, 0x373A, 35, 45, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);
				Effects.SendTargetParticles(from, 0x376A, 35, 45, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);
				from.SendMessage("An artifact is beamed into your backpack!");
                this.Delete();
			}

		}

		public override void Serialize ( GenericWriter writer)
		{
			base.Serialize ( writer );

			writer.Write ( (int) 0);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize ( reader );

			int version = reader.ReadInt();
		}
	}
}