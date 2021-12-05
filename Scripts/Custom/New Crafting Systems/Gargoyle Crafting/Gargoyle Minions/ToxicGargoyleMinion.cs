using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class ToxicGargoyleMinion : BaseCreature
	{
		[Constructable]
		public ToxicGargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Toxic Gargoyle Minion";
			Body = 4;
            Hue = 1272;
            BaseSoundID = 0x174;

			 SetStr(320, 525);
		     SetDex( 356, 525 );
		     SetInt( 450, 880 );

             SetHits(3000, 4000);

             SetDamage( 300, 500 );

			SetDamageType( ResistanceType.Physical, 80 );
            SetDamageType(ResistanceType.Poison, 100);

            SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 85 );
			SetResistance( ResistanceType.Energy, 60 );

			
			SetSkill( SkillName.EvalInt, 115.6, 126.5 );
			SetSkill( SkillName.Magery, 115.6, 128.5 );
			SetSkill( SkillName.Meditation, 180.0 );
			SetSkill( SkillName.MagicResist, 120.1, 150.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

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
		

		public ToxicGargoyleMinion( Serial serial ) : base( serial )
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
