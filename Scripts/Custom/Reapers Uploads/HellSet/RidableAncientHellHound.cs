using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an ancient hellhound corpse" )]
    public class RidableAncientHellHound1 : BaseMount
	{
        [Constructable]
        public RidableAncientHellHound1()
            : this("an ancient hellhound")
        {
        }

		[Constructable]
        public RidableAncientHellHound1(string name)
            : base(name, 0x42D, 0x3EC9, AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			BaseSoundID = 229;

            SetStr( 5402, 5552);
            SetDex( 1243, 1322);
            SetInt( 3432, 3500);

			SetHits( 232500, 242000 );
			SetMana( 10045, 15000 );

			SetDamage( 175, 200 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 150 );
			

			SetResistance( ResistanceType.Physical, 97, 100 );
			SetResistance( ResistanceType.Fire, 100, 100 );
			SetResistance( ResistanceType.Cold, 97, 100 );
			SetResistance( ResistanceType.Poison, 97, 100 );
			SetResistance( ResistanceType.Energy, 97, 100 );

			SetSkill( SkillName.MagicResist, 125.1, 135.0 );
			SetSkill( SkillName.Magery, 125.1, 135.0 );
			SetSkill( SkillName.Necromancy, 125.1, 135.0 );
			SetSkill( SkillName.EvalInt, 125.1, 135.0 );
			SetSkill( SkillName.SpiritSpeak, 125.1, 135.0 );
			SetSkill( SkillName.Anatomy, 105.1, 128.0 );
			SetSkill( SkillName.Tactics, 102.1, 120.0 );
			SetSkill( SkillName.Wrestling, 111.1, 119.0 );
			
			Skills[SkillName.Anatomy].Cap = 300;
			Skills[SkillName.MagicResist].Cap = 140;
			Skills[SkillName.Tactics].Cap = 300;
			Skills[SkillName.Wrestling].Cap = 140;
			Skills[SkillName.Magery].Cap = 140;
			Skills[SkillName.EvalInt].Cap = 300;
			Skills[SkillName.Necromancy].Cap = 140;
			Skills[SkillName.SpiritSpeak].Cap = 300;
			
			Fame = 24000;
			Karma = -24000;
			
			ControlSlots = 4;
			Tamable = false;
			SetSpecialAbility(SpecialAbility.AngryFire);
			
			
			if ( Utility.RandomDouble() <0.05 )
		    {
					switch (Utility.Random(3))
                {
                case 0:
				PackItem(new BeltOfHell());break;
				case 1:
				PackItem(new HellsTotem());break;
				case 2:
				PackItem(new BootsOfHell());break;
				}
			}		
			else if ( Utility.RandomDouble() >0.40 )
			{
					switch (Utility.Random(16))
				{
                case 0:
				PackItem(new ArmsOfHell());break;
				case 1:
				PackItem(new ChestOfHell());break;
				case 2:
				PackItem(new CapOfHell());break;
				case 3:
				PackItem(new GorgetOfHell());break;
				case 4:
				PackItem(new GlovesOfHell());break;
				case 5:
				PackItem(new FemaleChestOfHell());break;
				case 6:
				PackItem(new SkirtOfHell());break;
				case 7:
				PackItem(new LegsOfHell());break;
				}
			}
			
			if ( Utility.RandomDouble() <0.01 )
		    {
				switch (Utility.Random(2))
				{
                case 0: PackItem(new HellsBow());break;
				case 1: PackItem(new HellsSoulGlaive()); break;
				}
			}
				
			switch (Utility.Random(25))
            {
                case 0:
				Tamable = true;
				MinTameSkill = 135.1;
				SetHits( 4350, 5000 );
                break;
            }
		}

        public RidableAncientHellHound1(Serial serial)
            : base(serial)
        {
        }
		public override bool CanAngerOnTame {  get { return true; } }
		public override bool SubdueBeforeTame { get {  return true; } }
        public override PackInstinct PackInstinct{ get { return PackInstinct.Canine | PackInstinct.Daemon; } }
		public override bool BardImmune { get{ return true; } }
		public override Poison PoisonImmune { get{ return Poison.Greater; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 16; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override double BonusPetDamageScalar { get { return (Core.SE) ? 20.0 : 1.0; } }
		public override bool HasBreath { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 1);
        }
		
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
        {
            if ( from is BaseCreature )
            {
				if ( damage >= 300)				
                damage = 300;			
            }
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
