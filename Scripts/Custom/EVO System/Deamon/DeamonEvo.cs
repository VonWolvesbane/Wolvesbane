using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	[CorpseName( "a deamon corpse" )]
	public class EvoDeamon : BaseEvo, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return DeamonSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new DeamonEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return true; } }
		public override Type GetEvoDustType() { return typeof( DeamonDust ); }

        //UOSI - removed as of the Oct 2019 merge
		//public override bool HasBreath{ get{ return true; } }

		public EvoDeamon( string name ) : base( name, AIType.AI_Mage, 0.01 )
		{
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

		public EvoDeamon( Serial serial ) : base( serial )
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
		}
	}
}
