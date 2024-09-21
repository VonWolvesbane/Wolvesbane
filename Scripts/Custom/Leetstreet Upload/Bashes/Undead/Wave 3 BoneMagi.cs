using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute bone mage corpse")]
    public class CuteBoneMage : BaseCreature
    {
        [Constructable]
        public CuteBoneMage() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Bone Mage";
            Body = 0x94; // Body ID for a lich-like appearance
            Hue = 1153; // Custom hue

            SetStr(800);
            SetDex(900);
            SetInt(3000); // High intelligence for spellcasting
            SetHits(80000); // Hits
            SetStam(900); // Stamina
            SetMana(3000); // Mana

            SetDamage(60, 90); // Min and Max damage

            SetSkill(SkillName.Wrestling, 200.0);
            SetSkill(SkillName.Tactics, 200.0);
            SetSkill(SkillName.Magery, 300.0);

            Fame = 4000; // Optional: Adjust based on desired difficulty
            Karma = -4000; // Optional: Adjust as needed

            VirtualArmor = 80; // Adjust based on desired difficulty
        }

        public CuteBoneMage(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && InRange(Combatant, 10))
            {
                // Cast a spell if there's a combatant
                if (Utility.RandomDouble() < 0.2) // 20% chance to cast a spell
                {
                    CastMagic();
                }
            }
        }

        private void CastMagic()
        {
            // Example magic spell: Fireball
            if (Mana >= 20)
            {
                Mana -= 20; // Cost of the spell
                if (Combatant is Mobile target)
                {
                    target.SendMessage("The Cute Bone Mage casts a fireball!");
                    // Apply damage to the target
                    int damage = Utility.Random(40, 60);
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
