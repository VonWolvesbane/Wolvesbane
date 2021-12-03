
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class ClawsOfWolverine : Tekagi
  {
		public override int MinDamage{ get{ return 20; } }
		public override int MaxDamage{ get{ return 25; } }
		public override int DefMaxRange{ get{ return 3; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

      [Constructable]
		public ClawsOfWolverine()
		{
          Name = "Claws of Wolverine";
            Hue = 0;
          
	  Layer = Layer.TwoHanded;
      
      WeaponAttributes.HitLeechHits = 50;    
      WeaponAttributes.HitLowerAttack = 25;
      WeaponAttributes.HitLowerDefend = 25;      
      WeaponAttributes.SelfRepair = 25;
      Attributes.SpellChanneling = 1;
      Attributes.AttackChance = 55;
            Attributes.RegenHits = 10;
            Attributes.BonusHits = 100;
            Attributes.BonusStr = 50;
            Attributes.RegenStam = 10;
      Attributes.DefendChance = 35;
      
      Attributes.WeaponDamage = 30;
      Attributes.WeaponSpeed = 40;
    
		
		}
		
		
		public ClawsOfWolverine( Serial serial ) : base( serial )
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
