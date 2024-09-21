using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute lich lord corpse")]
    public class CuteLichLord : BaseCreature
    {
        [Constructable]
        public CuteLichLord() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Lich Lord";
            Body = 0x18; // Lich-like body ID
            Hue = 2844; // Custom hue

            SetStr(1000);
            SetDex(1200);
            SetInt(4000); // High intelligence for powerful spells
            SetHits(250000); // Total hits
            SetStam(2000); // Stamina

            SetDamage(75, 110); // Min and Max damage

            SetSkill(SkillName.Wrestling, 250.0);
            SetSkill(SkillName.Tactics, 250.0);
            SetSkill(SkillName.Parry, 250.0);
            SetSkill(SkillName.Anatomy, 250.0);
            SetSkill(SkillName.Magery, 500.0); // Strong magic capability

            Fame = 5000; // Optional: Adjust based on desired difficulty
            Karma = -5000; // Optional: Adjust as needed

            VirtualArmor = 100; // Adjust based on desired difficulty
        }

        public CuteLichLord(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && InRange(Combatant, 10))
            {
                // Cast a spell if there's a combatant
                if (Utility.RandomDouble() < 0.4) // 40% chance to cast a spell
                {
                    CastMagic();
                }
            }
        }

        private void CastMagic()
        {
            // Example spell: Greater Explosion
            if (Mana >= 100) // Cost of the spell
            {
                Mana -= 100;
                if (Combatant is Mobile target)
                {
                    target.SendMessage("The Cute Lich Lord conjures a devastating blast!");
                    // Apply damage to the target
                    int damage = Utility.Random(300, 400); // Stronger damage
                    DoHarmful(target);
                    target.Damage(damage, this);
                    Effects.SendTargetParticles(target, 0x36D4, 1, 30, 0x227, 0, 5020, EffectLayer.Waist, 0);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
