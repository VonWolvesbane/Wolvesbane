using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a scorpion corpse")]
    public class Leprechauntame : HordeMinion
    {
		
		public static TimeSpan TalkDelay = TimeSpan.FromSeconds(30.0); //the delay between talks is 10 seconds
        public DateTime m_NextTalk;
		
		        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.Now >= m_NextTalk && InRange(m, 4) && InLOS(m)) // check if it's time to talk & mobile in range & in los.
            {
                m_NextTalk = DateTime.Now + TalkDelay; // set next talk time 
                switch (Utility.Random(3))
                {
                    case 0: Say("You try'in to get your hands on ma lucky charms?"); //make it say ...
                        PlaySound(1066); //play giggle sound
                        break;
				};
			}
		}
        [Constructable]
        public Leprechauntame()
            : base()
        {
            Name = "A Leprechaun";
            Body = 776;
            BaseSoundID = 0x600;
			Hue = 72;
			NameHue = 72;

            SetStr(482, 500);
            SetDex(258, 295);
            SetInt(136, 150);

            SetHits(500, 750);
            SetMana(100, 200);

            SetDamage(50, 75);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 50, 55);
            SetResistance(ResistanceType.Fire, 50, 55);
            SetResistance(ResistanceType.Cold, 50, 55);
            SetResistance(ResistanceType.Poison, 55, 50);
            SetResistance(ResistanceType.Energy, 90, 100);

            SetSkill(SkillName.MagicResist, 30.1, 35.0);
            SetSkill(SkillName.Tactics, 60.3, 75.0);
            SetSkill(SkillName.Wrestling, 50.3, 65.0);
			
			Skills[SkillName.Wrestling].Cap = 300;
			Skills[SkillName.Tactics].Cap = 300;
			Skills[SkillName.Anatomy].Cap = 300;
			

            Fame = 222000;
            Karma = -2000;

            VirtualArmor = 55;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 90;
			


            PackItem(new Gold(15000));
			
        }
		
        public Leprechauntame(Serial serial)
            : base(serial)
        {
        }
		
        public override void GenerateLoot()
        {
            AddLoot( LootPack.Poor );
            AddLoot( LootPack.Average, 2 ); 
			
			Item item = new LuckyCharm();
			
			if ( Utility.RandomDouble() < 0.05 )
					{
					switch ( Utility.Random( 1 ) )
					{
				case 0: item = new LuckyCharm(); break;
					}
				}
        }
		
        public override int Meat
        {
            get
            {
                return 1;
            }
        }
		
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Gold;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }

		 public override void OnGaveMeleeAttack( Mobile defender )
        {
			if (defender is Mobile)
            {
				if ( 0.1 > Utility.RandomDouble())//10% of the time this weapon will set a defenders hit points to 0 almost killing it
            {
                  defender.Hits -=500;
				  //defender.Say( "Recieved A deadly Blow" ); 
				  this.Say("{1}. How did ya like my lucky hit?", this.Name, defender.Name);
                        }
			}
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0 && (AbilityProfile == null || AbilityProfile.MagicalAbility == MagicalAbility.None))
            {
                SetMagicalAbility(MagicalAbility.Poisoning);
            }
        }
    }
}