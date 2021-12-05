//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 3:53:17 PM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Sister Lady Medusa corpse" )]
	public class SisterLadyMedusa : AncientWyrm
	{
		[Constructable]
		public SisterLadyMedusa()
		{
			this.Name = "Sister Lady Medusa";
			this.Body = 728;
			//this.BaseSoundID = 362;

			this.SetStr(1096, 1185);
			this.SetDex(155, 175);
			this.SetInt(686, 775);

			this.SetHits(2500, 3000);

			this.SetDamage(55, 75);
			
			this.SetDamageType(ResistanceType.Energy, 75);
			this.SetDamageType(ResistanceType.Poison, 75);
            this.SetDamageType(ResistanceType.Cold, 75);
			this.SetDamageType(ResistanceType.Physical, 75);
			this.SetDamageType(ResistanceType.Fire, 75);

			this.SetResistance(ResistanceType.Physical, 65, 75);
			this.SetResistance(ResistanceType.Fire, 80, 90);
			this.SetResistance(ResistanceType.Cold, 70, 80);
			this.SetResistance(ResistanceType.Poison, 60, 70);
			this.SetResistance(ResistanceType.Energy, 60, 70);

			this.SetSkill(SkillName.EvalInt, 80.1, 100.0);
			this.SetSkill(SkillName.Magery, 80.1, 100.0);
			this.SetSkill(SkillName.Meditation, 52.5, 75.0);
			this.SetSkill(SkillName.MagicResist, 100.5, 150.0);
			this.SetSkill(SkillName.Tactics, 97.6, 100.0);
			this.SetSkill(SkillName.Wrestling, 97.6, 100.0);

			this.Fame = 22500;
			this.Karma = -22500;

			this.VirtualArmor = 80;




		}


		public SisterLadyMedusa( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

	}
}
