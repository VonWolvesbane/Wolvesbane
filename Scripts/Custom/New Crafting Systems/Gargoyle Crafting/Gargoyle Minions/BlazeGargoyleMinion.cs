using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle minion corpse" )]
	public class BlazeGargoyleMinion : BaseCreature
	{
		[Constructable]
		public BlazeGargoyleMinion() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Blaze Gargoyle Minion";
			Body = 4;
            Hue = 1161;
            BaseSoundID = 0x174;

			 SetStr(450, 675);
		     SetDex( 456, 675 );
		     SetInt( 750, 1080 );

             SetHits(5000, 7000);

             SetDamage( 400, 600 );

			SetDamageType( ResistanceType.Physical, 85 );
            SetDamageType(ResistanceType.Fire, 100);

            SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 90 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 60 );

			
			SetSkill( SkillName.EvalInt, 100.6, 120.5 );
			SetSkill( SkillName.Magery, 100.6, 120.5 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.MagicResist, 110.1, 120.0 );
			SetSkill( SkillName.Tactics, 135.0 );
			SetSkill( SkillName.Wrestling, 135.0 );

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
		

		public BlazeGargoyleMinion( Serial serial ) : base( serial )
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
