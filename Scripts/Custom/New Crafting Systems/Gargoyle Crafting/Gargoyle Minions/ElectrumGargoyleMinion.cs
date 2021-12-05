using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class ElectrumGargoyleMinion : BaseCreature
	{
		[Constructable]
		public ElectrumGargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Electrum Gargoyle Minion";
			Body = 4;
            Hue = 1278;
            BaseSoundID = 0x174;

			 SetStr(320, 525);
		     SetDex( 356, 525 );
		     SetInt( 450, 880 );

             SetHits(7000, 10000);

             SetDamage( 600, 800 );

			SetDamageType( ResistanceType.Physical, 100 );
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance( ResistanceType.Physical, 85 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 90 );

			
			SetSkill( SkillName.EvalInt, 125.6, 140.5 );
			SetSkill( SkillName.Magery, 125.6, 140.5 );
			SetSkill( SkillName.Meditation, 170.0 );
			SetSkill( SkillName.MagicResist, 120.1, 160.0 );
			SetSkill( SkillName.Tactics, 160.0 );
			SetSkill( SkillName.Wrestling, 160.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 24;
		

		    Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 0;
			{
                             
			}
        }

		
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
		

		public ElectrumGargoyleMinion( Serial serial ) : base( serial )
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
