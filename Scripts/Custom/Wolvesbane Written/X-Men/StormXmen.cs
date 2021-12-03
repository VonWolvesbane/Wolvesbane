// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Storm")]
    public class StormXMen : BaseCreature
    {
        [Constructable]
        public StormXMen()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Storm";
	        Title = "Master of the Weather (X-Men)";
            Body = 401;
            Female = true;
            Hue = 1545;
            HairItemID = 8252;
            HairHue = 1153;
            

            SetStr(1350, 3400);
            SetDex(1150, 1200);
            SetInt(1150, 1200);

            SetHits(100000, 150000);

            SetDamage(55, 75);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 1);
            SetResistance(ResistanceType.Fire, 0, 1);
            SetResistance(ResistanceType.Poison, 0, 1);
            SetResistance(ResistanceType.Energy, 0, 1);

            SetSkill(SkillName.EvalInt, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 115.0, 117.5);
            SetSkill(SkillName.Wrestling, 130.2, 145.0);
            SetSkill(SkillName.Magery, 145.0, 175.0);           
            SetSkill(SkillName.Healing, 100.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 5000, 20000 );

			TunicofXMenUniform Chest = new TunicofXMenUniform();
			Chest.Movable = false;
            Chest.ItemID = 7172;
            Chest.Hue = 1153;
            AddItem(Chest);
            				
			LegsofXMenUniform Legs = new LegsofXMenUniform();
			Legs.Movable = false;
            Legs.Hue = 1153;
            AddItem(Legs);
			
			GlovesofXMenUniform Gloves = new GlovesofXMenUniform();
			Gloves.Movable = false;
            Gloves.Hue = 1153;
            AddItem(Gloves);

            BootsofXMenUniform Boots = new BootsofXMenUniform();
            Boots.Movable = false;
            Boots.Hue = 1153;
            AddItem(Boots);

            ArmsofXMenUniform Arms = new ArmsofXMenUniform();
            Arms.Movable = false;
            Arms.Hue = 1153;
            AddItem(Arms);

            StormsCloak Cloak = new StormsCloak();
            Cloak.Movable = false;
            Cloak.Hue = 1153;
            AddItem(Cloak);
            

	  }

        public override void GenerateLoot()
        {
            
                AddLoot(LootPack.FilthyRich);
            

            if (Utility.RandomDouble() < 0.15)
                switch (Utility.Random(1))
                {
                    case 0: PackItem(new StormsCloak()); break;
                }
        }

        public StormXMen(Serial serial)
            : base(serial)
        {
        }
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }
        public override bool AlwaysMurderer { get { return true; } }

        public void SpawnStorms(Mobile target)
        {
            Map map = Map;

            if (map == null)
                return;

            int storms = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is ThunderStorm || m is SnowStorm || m is IceStorm)
                    ++storms;
            }

            if (storms < 6)
            {
                PlaySound(0x3D);

                int newStorms = Utility.RandomMinMax(1, 4);

                for (int i = 0; i < newStorms; ++i)
                {
                    BaseCreature storm;

                    switch (Utility.Random(5))
                    {
                        default:
                        case 0:
                        case 1: storm = new ThunderStorm(); break;
                        case 2:
                        case 3: storm = new SnowStorm(); break;
                        case 4: storm = new IceStorm(); break;
                    }

                    storm.Team = this.Team;

                    bool validLocation = false;
                    Point3D loc = this.Location;

                    for (int j = 0; !validLocation && j < 10; ++j)
                    {
                        int x = this.X + Utility.Random(3) - 1;
                        int y = this.Y + Utility.Random(3) - 1;
                        int z = map.GetAverageZ(x, y);

                        if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                            loc = new Point3D(x, y, this.Z);
                        else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                            loc = new Point3D(x, y, z);
                    }
					
                    storm.MoveToWorld(loc, map);
                    storm.Combatant = target;
                }
            }
        }

        public void DoSpecialAbility(Mobile target)
        {
            if (0.1 >= Utility.RandomDouble()) // 10% chance to more ratmen
                this.SpawnStorms(target);
        }
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            this.DoSpecialAbility(attacker);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            this.DoSpecialAbility(defender);
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
