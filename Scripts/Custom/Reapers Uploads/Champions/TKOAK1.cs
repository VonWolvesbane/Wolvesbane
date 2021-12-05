using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.Mobiles
{
	[CorpseName( "A Godly Corpse" )]
	public class TKOAK : BaseCreature //make it use the creature files/settings
	{

		public override WeaponAbility GetWeaponAbility() 
		{
			int ability = Utility.Random(3);
			if (ability == 1)
				return WeaponAbility.MortalStrike;
			else if (ability == 2)
				return WeaponAbility.MortalStrike;
			else
				return WeaponAbility.MortalStrike;
		}
		

		//private bool i_ChampionSpawn;
		//public bool ChampionSpawn{ get{ return i_ChampionSpawn; } set { i_ChampionSpawn = value; InvalidateProperties(); } }

		[Constructable]
		public TKOAK( /*bool i_ChampionSpawn */) 
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "The Keeper of Advanced Knowledge "; //the name players will see
			Body = 1253; //how it look like in game
			BaseSoundID = 42; //what sound he makes (I still have problems with sound :(

			//ChampionSpawn = i_ChampionSpawn; //set if it spawn with champ spawn or normal spawn

			SetStr( 6401, 6420 ); //set stats
			SetDex( 4481, 4490 );
			SetInt( 3201, 3220 );

			SetHits( 550000, 570000 ); //set hp

			SetDamage( 75, 90 ); //set how much damage ~

			SetDamageType( ResistanceType.Physical, 0 ); //which damage type it does
			SetDamageType( ResistanceType.Cold, 175 );

			SetResistance( ResistanceType.Physical, 80, 90 ); //what resists it have
			SetResistance( ResistanceType.Fire, 75, 80 );
			SetResistance( ResistanceType.Cold, 75, 80 );
			SetResistance( ResistanceType.Poison, 75, 80 );
			SetResistance( ResistanceType.Energy, 75, 80 );

			SetSkill( SkillName.Anatomy, 175.0 );
			SetSkill( SkillName.Tactics, 250.0 );
			SetSkill( SkillName.Wrestling, 250.0 );

			Fame = 30000; //its fame/karma
			Karma = -30000;
            
			Female = false;
			
			VirtualArmor = 50;

		}
		public override void GenerateLoot() 
		{
			AddLoot(LootPack.SuperBoss, 3);
		}
		
			public TKOAK( Serial serial ) : base( serial ) 
		{
		}
		public override bool BardImmune{ get{ return true; } } 
		public override bool AutoDispel{ get{ return true; } } 
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } } 

		public override bool AlwaysMurderer{ get{ return true; } }
		
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( attacker is BaseCreature )
            {
				attacker.Damage(Utility.Random(600, 800), this); 
			}
		}
		
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
        {
            if ( from is BaseCreature )
            {
				if ( damage >= 100)
					
                damage = 100;
            }
         }	
		 
		 public override void OnGaveMeleeAttack( Mobile defender )
        {
			if (defender is Mobile)
            {
				if ( 0.1 > Utility.RandomDouble())//10% of the time this weapon will set a defenders hit points to 0 almost killing it
            {
                  defender.Hits -=500;
				  defender.Say( "Recieved A deadly Blow" ); 
            }
			}
		}	
		public override void OnDeath(Container c)
        {
					base.OnDeath(c);
            List<DamageStore> rights = GetLootingRights();            

            foreach (Mobile m in rights.Select(x => x.m_Mobile).Distinct())
            {
                if (m is PlayerMobile)
                {
				int level;
				double random = Utility.RandomDouble();
				if ( 0.05 >= random ) // select the level of the ps
					level = 140;
				else if ( 0.25 >= random )
					level = 130;
				else if ( 0.40 >= random )
					level = 125;
				else
					level = 120;
								
				switch ( Utility.Random( 44 )) // select which skill to use in the ps
				{ 
					case 0: m.AddToBackpack( new PowerScroll( SkillName.Alchemy, level ) ); break; 
					case 1: m.AddToBackpack( new PowerScroll( SkillName.Anatomy, level ) ); break;  
					case 2: m.AddToBackpack( new PowerScroll( SkillName.AnimalLore, level ) ); break; 
					case 3: m.AddToBackpack( new PowerScroll( SkillName.ItemID, level ) ); break;  
					case 4: m.AddToBackpack( new PowerScroll( SkillName.ArmsLore, level ) ); break;  
					case 5: m.AddToBackpack( new PowerScroll( SkillName.Parry, level ) ); break; 
					case 6: m.AddToBackpack( new PowerScroll( SkillName.Blacksmith, level ) ); break; 
					case 7: m.AddToBackpack( new PowerScroll( SkillName.Fletching, level ) ); break;  
					case 8: m.AddToBackpack( new PowerScroll( SkillName.Peacemaking, level ) ); break;  
					case 9: m.AddToBackpack( new PowerScroll( SkillName.Carpentry, level ) ); break;  
					case 10: m.AddToBackpack( new PowerScroll( SkillName.DetectHidden, level ) ); break; 
					case 11: m.AddToBackpack( new PowerScroll( SkillName.Discordance, level ) ); break; 
					case 12: m.AddToBackpack( new PowerScroll( SkillName.EvalInt, level ) ); break; 
					case 13: m.AddToBackpack( new PowerScroll( SkillName.Healing, level ) ); break;  
					case 14: m.AddToBackpack( new PowerScroll( SkillName.Hiding, level ) ); break;  
					case 15: m.AddToBackpack( new PowerScroll( SkillName.Provocation, level ) ); break; 
					case 16: m.AddToBackpack( new PowerScroll( SkillName.Inscribe, level ) ); break; 
					case 17: m.AddToBackpack( new PowerScroll( SkillName.Magery, level ) ); break;  
					case 18: m.AddToBackpack( new PowerScroll( SkillName.MagicResist, level ) ); break;  
					case 19: m.AddToBackpack( new PowerScroll( SkillName.Tactics, level ) ); break;
					case 20: m.AddToBackpack( new PowerScroll( SkillName.Musicianship, level ) ); break; 
					case 21: m.AddToBackpack( new PowerScroll( SkillName.Poisoning, level ) ); break; 
					case 22: m.AddToBackpack( new PowerScroll( SkillName.Archery, level ) ); break; 
					case 23: m.AddToBackpack( new PowerScroll( SkillName.SpiritSpeak, level ) ); break;  
					case 24: m.AddToBackpack( new PowerScroll( SkillName.Tailoring, level ) ); break;  
					case 25: m.AddToBackpack( new PowerScroll( SkillName.AnimalTaming, level ) ); break; 
					case 26: m.AddToBackpack( new PowerScroll( SkillName.Tinkering, level ) ); break; 
					case 27: m.AddToBackpack( new PowerScroll( SkillName.Veterinary, level ) ); break;  
					case 28: m.AddToBackpack( new PowerScroll( SkillName.Swords, level ) ); break;  
					case 29: m.AddToBackpack( new PowerScroll( SkillName.Macing, level ) ); break;
					case 30: m.AddToBackpack( new PowerScroll( SkillName.Fencing, level ) ); break; 
					case 31: m.AddToBackpack( new PowerScroll( SkillName.Wrestling, level ) ); break; 
					case 32: m.AddToBackpack( new PowerScroll( SkillName.Lumberjacking, level ) ); break; 
					case 33: m.AddToBackpack( new PowerScroll( SkillName.Mining, level ) ); break;  
					case 34: m.AddToBackpack( new PowerScroll( SkillName.Meditation, level ) ); break;  
					case 35: m.AddToBackpack( new PowerScroll( SkillName.Necromancy, level ) ); break; 
					case 36: m.AddToBackpack( new PowerScroll( SkillName.Focus, level ) ); break; 
					case 37: m.AddToBackpack( new PowerScroll( SkillName.Chivalry, level ) ); break;  
					case 38: m.AddToBackpack( new PowerScroll( SkillName.Bushido, level ) ); break;  
					case 39: m.AddToBackpack( new PowerScroll( SkillName.Ninjitsu, level ) ); break;
					case 40: m.AddToBackpack( new PowerScroll( SkillName.Spellweaving, level ) ); break; 
					case 41: m.AddToBackpack( new PowerScroll( SkillName.Mysticism, level ) ); break; 
					case 42: m.AddToBackpack( new PowerScroll( SkillName.Imbuing, level ) ); break; 
					case 43: m.AddToBackpack( new PowerScroll( SkillName.Throwing, level ) ); break;  
				}
				m.SendMessage("You have recieved A Scroll of power for your efforts!"); 
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