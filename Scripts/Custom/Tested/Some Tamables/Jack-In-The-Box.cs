//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 11:14:36 AM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Jack In The Box corpse" )]
	public class JackInTheBox : GreaterDragon
	{
		[Constructable]
		public JackInTheBox()
		{
			this.Name = "Jack-In-The-Box";
            this.Body = (1428);
            //this.BaseSoundID = 362;

            this.SetStr(1025, 1425);
            this.SetDex(81, 148);
            this.SetInt(475, 675);

            this.SetHits(1000, 2000);
            
            this.SetDamage(24, 33);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 60, 85);
            this.SetResistance(ResistanceType.Fire, 65, 90);
            this.SetResistance(ResistanceType.Cold, 40, 55);
            this.SetResistance(ResistanceType.Poison, 40, 60);
            this.SetResistance(ResistanceType.Energy, 50, 75);

            this.SetSkill(SkillName.Meditation, 0);
            this.SetSkill(SkillName.EvalInt, 110.0, 140.0);
            this.SetSkill(SkillName.Magery, 110.0, 140.0);
            this.SetSkill(SkillName.Poisoning, 0);
            this.SetSkill(SkillName.Anatomy, 0);
            this.SetSkill(SkillName.MagicResist, 110.0, 140.0);
            this.SetSkill(SkillName.Tactics, 110.0, 140.0);
            this.SetSkill(SkillName.Wrestling, 115.0, 145.0);

            this.Fame = 22000;
            this.Karma = -15000;

            this.VirtualArmor = 60;

            this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 104.7;




		}


		public JackInTheBox( Serial serial ) : base( serial )
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
