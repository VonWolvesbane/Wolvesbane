using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Le'Vel Eyetem")]
    public class LeVelEyetem : BaseCreature
    {
        [Constructable]
        public LeVelEyetem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Le'Vel Eyetem";
            this.Body = 75;
            this.BaseSoundID = 604;

            this.SetStr(1336, 1385);
            this.SetDex(196, 315);
            this.SetInt(131, 255);

            this.SetHits(12020, 12310);
            this.SetMana(1000);

            this.SetDamage(37, 83);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 90, 95);
            this.SetResistance(ResistanceType.Fire, 90, 95);
            this.SetResistance(ResistanceType.Cold, 90, 95);
            this.SetResistance(ResistanceType.Poison, 90, 95);
            this.SetResistance(ResistanceType.Energy, 90, 95);

            this.SetSkill(SkillName.MagicResist, 100.3, 125.0);
            this.SetSkill(SkillName.Tactics, 180.1, 200.0);
            this.SetSkill(SkillName.Wrestling, 180.1, 190.0);

            this.Fame = 45000;
            this.Karma = -45000;

            this.VirtualArmor = 85;
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
//Test 
			Item item = new LevelItemDeed(1);
			
			if ( Utility.RandomDouble() < 0.80 )
				{
					switch ( Utility.Random( 6 ) )
					{
						case 0: item = new LevelUpScroll(5); break;
						case 1: item = new LevelUpScroll(10); break;
						case 2: item = new LevelUpScroll(15); break;
						case 3: item = new LevelUpScroll(20); break;
						case 4: item = new LevelItemDeed(); break;
					}
				}
						
					switch ( Utility.Random( 3 ) )
					{
						case 0: c.DropItem(item); break;
					}
					
		}						
//End Test
            /*switch (Utility.Random(10))
            {
                case 0: c.DropItem(new LevelItemDeed()); break;
            }
			if ( Utility.RandomDouble() < 0.05 )
					{
					switch ( Utility.Random( 1 ) )
						{
							case 0: new LevelUpScroll(5); break;
						}
					}							
				if ( Utility.RandomDouble() < 0.009 )
					{
					switch ( Utility.Random( 1 ) )
						{
							case 0: new LevelUpScroll(10); break;
						}
					}
        }*/
        public LeVelEyetem(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 4;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 3;
            }
        }
		public override bool HasAura { get { return !Controlled; } }
        public override int AuraRange { get { return 10; } }
        public override int AuraBaseDamage { get { return 10; } }
        public override int AuraFireDamage { get { return 100; } }
        public override int AuraColdDamage { get { return 0; } }
		public override void AuraEffect(Mobile m)
        {
            m.SendMessage("You feel intense preasure!"); //  : The intense cold is damaging you!
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Average);


            
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
