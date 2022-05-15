using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{		
//public virtual double HealDelay { get { return 0.5; } }
//public virtual double HealChance { get { return 1.0; } }
//public virtual double HealScalar { get { return 5.0; } }
	[CorpseName( "a Wolf corpse" )]
	public class WolfEvo : BaseChivMountEvo, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return WolfEvoSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new WolfEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return true; } }
		public override Type GetEvoDustType() { return typeof( WolfDust ); }

        //UOSI - removed as of the Oct 2019 merge
		//public override bool HasBreath{ get{ return true; } }

		public WolfEvo( string name ) : base( name, 277, 0x3E91 )
		{
			SetMana(900, 900);
			
            SetSpecialAbility(SpecialAbility.Heal);
			SetWeaponAbility(WeaponAbility.BleedAttack);			
        }

		public WolfEvo( Serial serial ) : base( serial )
		{
		}
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );			
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			// Temporary to fix up all existing wolves
			if (Stage >= 4)
			{
				SetSkill(SkillName.MagicResist, 200, 200);
				SetResistance(ResistanceType.Physical, 200);
				SetResistance(ResistanceType.Cold, 200);
				SetResistance(ResistanceType.Poison, 200);
				SetResistance(ResistanceType.Energy, 200);
				SetResistance(ResistanceType.Fire, 200);
			}
		}
	}
}
