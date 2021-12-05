using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class PlatinumGargoyleMinion : BaseCreature
	{
		[Constructable]
		public PlatinumGargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Platinum Gargoyle Minion";
			Body = 4;
            Hue = 1153;
            BaseSoundID = 0x174;

			 SetStr(700, 1000);
		     SetDex( 500, 1000 );
		     SetInt( 800, 1600 );

             SetHits(8000, 120000);

             SetDamage( 400, 800 );

            SetDamageType(ResistanceType.Physical, 95);
            SetDamageType(ResistanceType.Fire, 95);
            SetDamageType(ResistanceType.Cold, 95);
            SetDamageType(ResistanceType.Poison, 95);
            SetDamageType(ResistanceType.Energy, 95);

            SetResistance( ResistanceType.Physical, 85 );
			SetResistance( ResistanceType.Fire, 85 );
			SetResistance( ResistanceType.Cold, 85 );
			SetResistance( ResistanceType.Poison, 85 );
			SetResistance( ResistanceType.Energy, 85 );

			
			SetSkill( SkillName.EvalInt, 120.0, 240.0 );
			SetSkill( SkillName.Magery, 120.0, 240.0 );
			SetSkill( SkillName.Meditation, 240.0 );
			SetSkill( SkillName.MagicResist, 120.0, 240.0 );
			SetSkill( SkillName.Tactics, 240.0 );
			SetSkill( SkillName.Wrestling, 240.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 24;
		

		    Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 0;
			{
                             
			}
        }

		
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
		

		public PlatinumGargoyleMinion( Serial serial ) : base( serial )
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
