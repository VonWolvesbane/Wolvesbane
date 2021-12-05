// Crafted By ReApEr
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Spells;
using Server.Gumps;
using Server.ContextMenus;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a Scarlet Nimh corpse" )]
	public class ScarletNimh : BaseMount
	{
		[Constructable]
		public ScarletNimh() : this( "Scarlet Nimh" )
		{
			Hue = 0;
		}

		[Constructable]
		public ScarletNimh( string name ) : base( name, 0x59A , 0x3ECE , AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Scarlet Nimh";
			BaseSoundID = 362;


            SetStr(1911, 2140);
            SetDex(301, 330);
            SetInt(401, 500);

            SetHits(2880, 3300);
			SetMana(2000, 3000);

            SetDamage(95, 105);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Poison, 150);

            SetResistance(ResistanceType.Physical, 75, 98);
            SetResistance(ResistanceType.Fire, 85, 95);
            SetResistance(ResistanceType.Cold, 85, 95);
            SetResistance(ResistanceType.Poison, 100, 100);
            SetResistance(ResistanceType.Energy, 85, 100);

            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 50.1, 60.0);
            SetSkill(SkillName.Wrestling, 80.1, 120.0);
            SetSkill(SkillName.DetectHidden, 200.0);
			SetSkill(SkillName.Poisoning, 100.1, 120.0);
			
			Skills[SkillName.Anatomy].Cap = 200;
			Skills[SkillName.Poisoning].Cap = 200;
			Skills[SkillName.MagicResist].Cap = 200;
			Skills[SkillName.Tactics].Cap = 200;
			Skills[SkillName.Wrestling].Cap = 200;

            Fame = 15000;
            Karma = 15000;

            VirtualArmor = 40;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 111.1;
			
			m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 10 ) );
		}

		public ScarletNimh( Serial serial ) : base( serial )
		{
		}
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        
        public override double BonusPetDamageScalar { get { return Controlled ? 1.0 : (Core.SE) ? 3.0 : 1.0; } }
        public override bool AutoDispel { get { return !Controlled; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Hides { get { return 20; } }
        public override int Meat { get { return 19; } }
        public override int Scales { get { return 6; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override bool CanAngerOnTame { get { return true; } }
		
        public override ScaleType ScaleType
        {
            get
            {
                return (Utility.RandomBool() ? ScaleType.Black : ScaleType.White);
            }
        }


        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 2);
        }

        public override int GetIdleSound()
        {
            return 0x2C4;
        }

        public override int GetAttackSound()
        {
            return 0x2C0;
        }

        public override int GetDeathSound()
        {
            return 0x2C1;
        }

        public override int GetAngerSound()
        {
            return 0x2C4;
        }

        public override int GetHurtSound()
        {
            return 0x2C3;
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
		
		#region [Poison Attack]
        private DateTime m_NextAbilityTime;

         public override void OnActionCombat()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (DateTime.Now < m_NextAbilityTime || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 2) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 10));

            if (Utility.RandomBool())
            {
                this.FixedParticles(0x376A, 9, 32, 0x2539, EffectLayer.LeftHand);
                this.PlaySound(0x1DE);

                foreach (Mobile m in this.GetMobilesInRange(10))
              {

            if (m != this && m != this.ControlMaster && IsEnemy(m))
          {

            m.ApplyPoison(this, Poison.Deadly);
			}
		}
	}
 }
        #endregion
        public override Poison HitPoison
        {
            get
            {
                return (0.9 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly);
            }
        }
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}