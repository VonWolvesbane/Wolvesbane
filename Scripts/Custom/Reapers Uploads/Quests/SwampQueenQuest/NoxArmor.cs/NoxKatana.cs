

using System;
using Server;

namespace Server.Items
{ 
	public class NoxKatana: Katana
	{
		public override int ArtifactRarity{ get{ return 10; } }

        public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 600; } }
                
		public override int StrengthReq{ get{ return 100; } }
		public override int MinDamage{ get{ return 20; } }
		public override int MaxDamage{ get{ return 31; } }
		public override float Speed{ get{ return 2.50f; } }

        public override int DefMaxRange{ get{ return 1; } }
		public override int DefHitSound{ get{ return 1140; } }
		public override int DefMissSound{ get{ return 517; } }
	
	

		[Constructable]
		public NoxKatana()
		{
            Name = " Katana of the Swamp Queen";
			Hue = 677;

            Attributes.Luck = 200;
			Attributes.WeaponSpeed = 35;
			Attributes.WeaponDamage = 75;
			Attributes.SpellChanneling = 1;
            Attributes.AttackChance = 25;
            WeaponAttributes.SelfRepair = 10;
            WeaponAttributes.HitPoisonArea = 100;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.ResistPoisonBonus = 25;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = fire = cold = nrgy = chaos = direct = 0;
			pois = 100;
		}

		public NoxKatana( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}