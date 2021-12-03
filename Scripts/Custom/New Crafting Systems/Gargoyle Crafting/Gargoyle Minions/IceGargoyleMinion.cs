using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class IceGargoyleMinion : BaseCreature
	{
		[Constructable]
		public IceGargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Ice Gargoyle Minion";
			Body = 4;
            Hue = 1152;
            BaseSoundID = 0x174;

			 SetStr(420, 625);
		     SetDex( 456, 625 );
		     SetInt( 550, 980 );

             SetHits(4000, 6000);

             SetDamage( 350, 550 );

			SetDamageType( ResistanceType.Cold, 100 );
            SetDamageType(ResistanceType.Physical, 85);


            SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 85 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 60 );

			
			SetSkill( SkillName.EvalInt, 95.6, 120.5 );
			SetSkill( SkillName.Magery, 95.6, 120.5 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.MagicResist, 100.1, 120.0 );
			SetSkill( SkillName.Tactics, 130.0 );
			SetSkill( SkillName.Wrestling, 130.0 );

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
		

		public IceGargoyleMinion( Serial serial ) : base( serial )
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
