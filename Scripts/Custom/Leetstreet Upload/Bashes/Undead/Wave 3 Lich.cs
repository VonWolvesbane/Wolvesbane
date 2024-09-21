using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute lich corpse")]
    public class CuteLich : BaseCreature
    {
        [Constructable]
        public CuteLich() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Lich";
            Body = 0x18; // Lich-like body ID
            Hue = 2253; // Custom hue

            SetStr(800);
            SetDex(900);
            SetInt(900); // High intelligence for spellcasting
            SetHits(80000); // Hits
            SetStam(900); // Stamina

            SetDamage(60, 400); // Min and Max damage

            SetSkill(SkillName.Wrestling, 175.0);
            SetSkill(SkillName.Tactics, 175.0);
            SetSkill(SkillName.Parry, 175.0);
            SetSkill(SkillName.Anatomy, 175.0);
            SetSkill(SkillName.Magery, 175.0);
            SetSkill(SkillName.EvalInt, 175.0);

            Fame = 4000; // Optional: Adjust based on desired difficulty
            Karma = -4000; // Optional: Adjust as needed

            VirtualArmor = 80; // Adjust based on desired difficulty
        }

        public CuteLich(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && InRange(Combatant, 10))
            {
                // Cast a spell if there's a combatant
                if (Utility.RandomDouble() < 0.3) // 30% chance to cast a spell
                {
                    CastMagic();
                }
            }
        }

        private void CastMagic()
        {
            // Example spell: Greater Explosion
            if (Mana >= 50)
            {
                Mana -= 50; // Cost of the spell
                if (Combatant is Mobile target)
                {
                    target.SendMessage("The Cute Lich conjures a powerful explosion!");
                    // Apply damage to the target
                    int damage = Utility.Random(200, 400); // Stronger damage
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
