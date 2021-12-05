using System;
using Server.Items;

namespace Server.Mobiles 
{ 
    public class Kindred : BaseCreature 
    { 
        [Constructable] 
        public Kindred()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.05, 0.1)
        { 
		
		    Body = 0x191; 
            SpeechHue = Utility.RandomDyedHue(); 
            Title = "the Mage"; 
            Hue = Utility.RandomSkinHue(); 
			Name = "Kindred";
            SetStr(3386, 3400);
            SetDex(350, 350);
            SetInt(7610, 7775);
			
			SetHits (35000, 37500 );
			SetMana (1000000);

            SetDamage(55, 70);

            SetDamageType(ResistanceType.Energy, 150);

            SetResistance(ResistanceType.Physical, 98, 100);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.2, 100.0);
            SetSkill(SkillName.MagicResist, 460.2, 490.0);
            SetSkill(SkillName.Magery, 150.0);
            SetSkill(SkillName.EvalInt, 150.0);
            SetSkill(SkillName.Meditation, 420.0);

            Fame = 50000;
            Karma = 50000;

			Female = true;
            VirtualArmor = 80;

            Item sword = new BloodyKatana();
            sword.Movable = false;
			sword.Hue =2452;
            this.AddItem(sword);
			
			Item gloves = new GlovesOfHell();
            gloves.Movable = false;
			gloves.Hue =39;
            this.AddItem(gloves);
			
			Item robe = new UberRobe();
            robe.Movable = false;
			robe.Hue =2325;
            this.AddItem(robe);
			
			Item shield = new DaminocShield();
            shield.Movable = false;
			shield.Hue =2452;
            this.AddItem(shield);
			
			Item gorget = new KageMaruMask();
            gorget.Movable = false;
			gorget.Hue =2514;
            this.AddItem(gorget);
			
			Item cape = new VIPCloak();
            cape.Movable = false;
			cape.Hue =2050;
            this.AddItem(cape);
			
			Item belt = new BeltOfLostSouls();
            belt.Movable = false;
			belt.Hue =0;
            this.AddItem(belt);
			
			Item shoes = new SexiSandals();
            shoes.Movable = false;
			shoes.Hue =2909;
            this.AddItem(shoes);
			
            Utility.AssignRandomHair(this);
			
			if ( Utility.RandomDouble() >0.40 )
			{
					switch (Utility.Random(15))
				{
                case 0:
				PackItem(new WitchesArms());break;
				case 1:
				PackItem(new WitchesChest());break;
				case 2:
				PackItem(new WitchesGloves());break;
				case 3:
				PackItem(new WitchesGorget());break;
				case 4:
				PackItem(new WitchesHat());break;
				case 5:
				PackItem(new WitchesLegs());break;
				case 6:
				PackItem(new WitchesSkirt());break;				
				case 7:
				PackItem(new Rolodex());break;
				case 8: 
				PackItem(new MagesRevengeGarg()); break;
				case 9: 
				PackItem(new MagesRevenge()); break;
				}
			}
			
			else if ( Utility.RandomDouble() <0.05 )
		    {
				switch (Utility.Random(1))
				{
                case 0: PackItem(new WitchesArms());break;

				}
			}
        }

        public Kindred(Serial serial)
            : base(serial)
        { 
        }
		public override bool CanFlee { get { return false; } }
        public bool BlockReflect { get; set; }
        
        public override int Damage(int amount, Mobile from, bool informMount, bool checkDisrupt)
        {
            int dam = base.Damage(amount, from, informMount, checkDisrupt);

            if (!BlockReflect && from != null && dam > 0)
            {
                BlockReflect = true;
                AOS.Damage(from, this, dam, 0, 0, 0, 0, 0, 0, 200);
                BlockReflect = false;
                
                from.PlaySound(0x1F1);
            }

            return dam;
        }
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			
			BaseCreature c = attacker as BaseCreature;
			
			if (attacker is BaseCreature)
			{
				if ( 0.35 > Utility.RandomDouble())//35% of the time when hit by pet this will remove 541 from said pets health regardless of resists.
				{
                  attacker.Hits -=541;
				  attacker.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist ); //FlameStrike Effect
				  attacker.PlaySound( 0x208 );
                }

            /*if ( c is BaseCreature || this.Followers <= 5 )
            {
				
			if ( 0.05 >= Utility.RandomDouble() ) //5% Chance
				c.Controlled = true;
				c.ControlMaster = this;
				this.Combatant = null;
				c.ControlOrder = OrderType.Guard;
				c.IsBonded = false;
				
				this.Say("This creature is now mine");
		}*/
			}
				else if( 0.35 > Utility.RandomDouble())//35% of the time when hit by pet this will remove 541 from said pets health regardless of resists.
				{
                  attacker.Hits -=333;
				  attacker.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist ); //FlameStrike Effect
				  attacker.PlaySound( 0x208 );
                }
		}
	
			public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
        {
            if ( from is BaseCreature )
            {
				if ( damage >= 300)
					
                damage = 300;
		
            }
         }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Meager);
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
