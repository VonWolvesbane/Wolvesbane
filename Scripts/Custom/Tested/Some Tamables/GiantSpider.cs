//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/17/2019 11:31:33 AM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Giant Spider corpse" )]
	public class AGiantSpider : Nightmare
	{
		[Constructable]
		public AGiantSpider()
		{
			this.Name = "Giant Spider";
			this.Body = 173;
			//this.hue = 0;
			//this.BaseSoundId = 0;
			
			this.SetStr( 1096, 1185 );
			this.SetDex( 155, 175 );
			this.SetInt( 686, 775 );

			this.SetHits( 609, 609 );
			this.SetStam( 245, 245 );
			this.SetMana( 237, 237 );

			this.SetSkill( SkillName.Wrestling, 21.1, 110 );
			this.SetSkill( SkillName.Tactics, 51.1, 110 );
			this.SetSkill( SkillName.MagicResist, 30.0, 110 );
			this.SetSkill( SkillName.Anatomy, 30.0, 110 );
			
			this.SetDamageType(ResistanceType.Energy, 50);
			this.SetDamageType(ResistanceType.Poison, 50);
            this.SetDamageType(ResistanceType.Cold, 50);
			this.SetDamageType(ResistanceType.Physical, 50);
			this.SetDamageType(ResistanceType.Fire, 50);

			this.SetResistance( ResistanceType.Physical, 75, 75 );
			this.SetResistance( ResistanceType.Fire, 75, 75 );
			this.SetResistance( ResistanceType.Cold, 75, 75 );
			this.SetResistance( ResistanceType.Poison, 75, 75 );
			this.SetResistance( ResistanceType.Energy, 75, 75 );

			this.Fame = 22500;
			this.Karma = 22500;
			
			this.VirtualArmor = 80;
			
			this.Tamable = true;
            this.ControlSlots = 4;
            this.MinTameSkill = 95.1;




		}


		public AGiantSpider( Serial serial ) : base( serial )
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
