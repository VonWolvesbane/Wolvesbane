//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 3:45:29 PM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an The Grim Shadow Lord corpse" )]
	public class TheGrimShadowLord : Nightmare
	{
		[Constructable]
		public TheGrimShadowLord()
		{
			this.Name = "The Grim Shadow Lord";
			this.Body = 704;
			
			
			this.SetStr(496, 525);
            this.SetDex(86, 105);
            this.SetInt(86, 125);

            this.SetHits(298, 315);

            this.SetDamage(16, 22);
			
			this.SetDamageType(ResistanceType.Poison, 40);
            this.SetDamageType(ResistanceType.Cold, 40);
            this.SetDamageType(ResistanceType.Physical, 40);
            this.SetDamageType(ResistanceType.Fire, 40);
            this.SetDamageType(ResistanceType.Energy, 20);

            this.SetResistance(ResistanceType.Physical, 55, 65);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 30, 40);
            this.SetResistance(ResistanceType.Energy, 20, 30);
			
			this.SetSkill(SkillName.Meditation, 10.4, 50.0);
			this.SetSkill(SkillName.Poisoning, 10.4, 50.0);
            this.SetSkill(SkillName.Anatomy, 10.4, 50.0);
            this.SetSkill(SkillName.EvalInt, 10.4, 50.0);
            this.SetSkill(SkillName.Magery, 10.4, 50.0);
            this.SetSkill(SkillName.MagicResist, 85.3, 100.0);
            this.SetSkill(SkillName.Tactics, 97.6, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.5, 92.5);

            this.Fame = 14000;
            this.Karma = -14000;

            this.VirtualArmor = 60;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 95.1;




		}


		public TheGrimShadowLord( Serial serial ) : base( serial )
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
