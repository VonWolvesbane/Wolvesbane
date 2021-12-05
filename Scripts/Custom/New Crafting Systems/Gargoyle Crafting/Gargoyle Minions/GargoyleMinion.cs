using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class GargoyleMinion : BaseCreature
	{
		[Constructable]
		public GargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Gargoyle Minion";
			Body = 4;
            Hue = 0;
            BaseSoundID = 0x174;

			 SetStr(300, 500);
		     SetDex( 300, 500 );
		     SetInt( 500, 600 );

             SetHits(2000, 3000);

             SetDamage(150, 200 );

			SetDamageType( ResistanceType.Physical, 100 );
            

            SetResistance( ResistanceType.Physical, 85 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 60 );

			
			SetSkill( SkillName.EvalInt, 90.6, 120.5 );
			SetSkill( SkillName.Magery, 90.6, 120.5 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.MagicResist, 90.1, 120.0 );
			SetSkill( SkillName.Tactics, 95.0 );
			SetSkill( SkillName.Wrestling, 95.0 );

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
		

		public GargoyleMinion( Serial serial ) : base( serial )
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
