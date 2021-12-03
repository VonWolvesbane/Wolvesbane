//Crafted By ReApEr

using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Peacock corpse" )]
	public class Peacock : BaseMount
	{
		[Constructable]
		public Peacock() : this( "Peacock" )
		{
			Hue = 0;
		}

		[Constructable]
		public Peacock( string name ) : base( name, 0x5A0 , 0x3ECF , AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x1B;

			SetStr(496, 525);
            SetDex(116, 165);
            SetInt(4400, 6500);

            SetHits(391, 450);
			SetMana(14000, 15000);

            SetDamage(45, 77);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 25, 40);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 25, 40);

            SetSkill(SkillName.EvalInt, 80.1, 120.0);
            SetSkill(SkillName.Magery, 90.2, 120.0);
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 75.3, 110.0);
            SetSkill(SkillName.Tactics, 20.1, 22.5);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);
			
			Skills[SkillName.Anatomy].Cap = 120;
			Skills[SkillName.Magery].Cap = 200;
			Skills[SkillName.EvalInt].Cap = 300;
			Skills[SkillName.MagicResist].Cap = 300;
			Skills[SkillName.Tactics].Cap = 120;
			Skills[SkillName.Wrestling].Cap = 120;

            Fame = 9000;
            Karma = 9000;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 101.1;
		}

		public Peacock( Serial serial ) : base( serial )
		{
		}
       public override bool AllowMaleRider
        {
            get
            {
                return false;
            }
        }
        public override bool AllowMaleTamer
        {
            get
            {
                return false;
            }
        }
        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }
        public override TimeSpan MountAbilityDelay
        {
            get
            {
                return TimeSpan.FromMinutes(0.1);
            }
        }

        public override TribeType Tribe { get { return TribeType.Fey; } }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Horned;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }
        public override void OnDisallowedRider(Mobile m)
        {
            m.SendMessage("Your Peacock refuses to allow you to ride it."); // The unicorn refuses to allow you to ride it.
        }

        public override bool DoMountAbility(int damage, Mobile attacker)
        {
            if (Rider == null || attacker == null)	//sanity
                return false;

            if (Rider.Poisoned && ((Rider.Hits - damage) < 90))
            {
                Poison p = Rider.Poison;

                if (p != null)
                {
                    int chanceToCure = 10000 + (int)(Skills[SkillName.Magery].Value * 75) - ((p.RealLevel + 1) * (Core.AOS ? (p.RealLevel < 4 ? 3300 : 3100) : 1750));
                    chanceToCure /= 100;

                    if (chanceToCure > Utility.Random(100))
                    {
                        if (Rider.CurePoison(this))	//TODO: Confirm if mount is the one flagged for curing it or the rider is
                        {
                            Rider.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, true, "Your mount senses you are in danger and aids you with magic.");
                            Rider.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                            Rider.PlaySound(0x1E0);	// Cure spell effect.

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }
		public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
            {
                SetWeaponAbility(WeaponAbility.ArmorIgnore);
            }
        }
    }
}
