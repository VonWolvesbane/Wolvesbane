
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class StaffofGambit : BlackStaff
  {
		public override int OldMinDamage{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 20; } }
		public override int OldMaxDamage{ get{ return 25; } }
		public override int AosMaxDamage{ get{ return 25; } }
		public override int AosSpeed{ get{ return 45; } }
		public override int DefMaxRange{ get{ return 3; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

      [Constructable]
		public StaffofGambit()
		{
          Name = "Staff of Gambit";
            Hue = 0;
          
	  Layer = Layer.TwoHanded;
      
      WeaponAttributes.HitFireball = 75;    
      WeaponAttributes.HitMagicArrow = 75;
      WeaponAttributes.HitLowerDefend = 25;
            WeaponAttributes.HitPoisonArea = 35;
      WeaponAttributes.SelfRepair = 25;
      Attributes.SpellChanneling = 1;
      Attributes.AttackChance = 55;
            Attributes.BonusStr = 50;
            Attributes.RegenStam = 10;
      Attributes.DefendChance = 35;
      
      Attributes.WeaponDamage = 45;
      Attributes.WeaponSpeed = 60;
    
		
		}
		
		
		public StaffofGambit( Serial serial ) : base( serial )
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
