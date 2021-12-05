using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a mechanical bone demon corpse" )]
	public class MechanicalBoneDemon : BaseCreature
	{
		[Constructable]
		public MechanicalBoneDemon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Mechanical Bone Demon";
			Body = 308;
            Hue = 981;
			BaseSoundID = 0x48D;

			 SetStr(620, 925);
		     SetDex( 556, 725 );
		     SetInt( 450, 880 );

             SetHits(3000, 4000);

             SetDamage( 521, 2225 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 90 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 60 );

			
			SetSkill( SkillName.EvalInt, 177.6, 187.5 );
			SetSkill( SkillName.Magery, 137.6, 187.5 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.MagicResist, 90.1, 125.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 24;
		

		    Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 0;
			{
                             
			}
        }

		
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
		

		public MechanicalBoneDemon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
