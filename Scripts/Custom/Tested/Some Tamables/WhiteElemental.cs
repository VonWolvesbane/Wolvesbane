//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 5:13:40 PM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a white elemental corpse" )]
	public class WhiteElemental : WhiteWyrm
	{
		[Constructable]
		public WhiteElemental()
		{
			this.Name = "White Elemental";
			this.Body = 159;
			this.Hue = 1150;
			
			this.SetStr(721, 760);
            this.SetDex(101, 130);
            this.SetInt(386, 425);

            this.SetHits(433, 456);

            this.SetDamage(17, 25);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Cold, 50);

            this.SetResistance(ResistanceType.Physical, 55, 70);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 80, 90);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.EvalInt, 99.1, 100.0);
            this.SetSkill(SkillName.Magery, 99.1, 100.0);
            this.SetSkill(SkillName.MagicResist, 99.1, 100.0);
            this.SetSkill(SkillName.Tactics, 97.6, 100.0);
            this.SetSkill(SkillName.Wrestling, 90.1, 100.0);

            this.Fame = 18000;
            this.Karma = -18000;

            this.VirtualArmor = 64;

            this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 96.3;




		}


		public WhiteElemental( Serial serial ) : base( serial )
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
