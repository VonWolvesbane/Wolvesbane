//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 4:18:52 PM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Dr Geekazoid corpse" )]
	public class DrGeekazoid : WhiteWyrm
	{
		[Constructable]
		public DrGeekazoid()
		{
			this.Name = "Dr Geekazoid";
			this.Body = 1405;
			
			this.SetStr(721, 760);
            this.SetDex(101, 130);
            this.SetInt(386, 425);

            this.SetHits(2433, 4456);

            this.SetDamage(77, 125);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 50);

            this.SetResistance(ResistanceType.Physical, 55, 70);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 80, 90);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 99.1, 150.0);
            this.SetSkill(SkillName.Magery, 99.1, 150.0);
            this.SetSkill(SkillName.MagicResist, 99.1, 150.0);
            this.SetSkill(SkillName.Tactics, 97.6, 150.0);
            this.SetSkill(SkillName.Wrestling, 90.1, 150.0);

            this.Fame = 18000;
            this.Karma = -18000;

            this.VirtualArmor = 64;

            this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 116.3;




		}


		public DrGeekazoid( Serial serial ) : base( serial )
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
