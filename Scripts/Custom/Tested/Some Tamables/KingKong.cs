//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/16/2019 10:44:56 AM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an King Kong corpse" )]
	public class KingKong : AncientWyrm
	{
		[Constructable]
		public KingKong()
		{
			this.Name = "King Kong";
			this.Body = 1308;
			this.BaseSoundID = 0x9E;
			
			this.SetStr( 1096, 1185 );
			this.SetDex( 155, 175 );
			this.SetInt( 686, 775 );

			this.SetHits( 2500, 3000 );
			
			this.SetDamage(55, 75);
			
            this.SetSkill( SkillName.Wrestling, 65, 110 );
			this.SetSkill( SkillName.Tactics, 65, 110 );
			this.SetSkill( SkillName.EvalInt, 65, 110 );
			this.SetSkill( SkillName.Anatomy, 65, 110 );
			this.SetSkill( SkillName.MagicResist, 65, 110 );
			this.SetSkill( SkillName.Magery, 65, 110 );
			this.SetSkill( SkillName.Meditation, 65, 110 );
			this.SetSkill( SkillName.Poisoning, 65, 110 );

			this.SetResistance( ResistanceType.Physical, 65, 75 );
			this.SetResistance( ResistanceType.Fire, 65, 75 );
			this.SetResistance( ResistanceType.Cold, 65, 75 );
			this.SetResistance( ResistanceType.Poison, 65, 75 );
			this.SetResistance( ResistanceType.Energy, 65, 75 );

			this.SetDamageType(ResistanceType.Energy, 75);
			this.SetDamageType(ResistanceType.Poison, 75);
            this.SetDamageType(ResistanceType.Cold, 75);
			this.SetDamageType(ResistanceType.Physical, 75);
			this.SetDamageType(ResistanceType.Fire, 75);
			
			this.Fame = 22500;
			this.Karma = 22500;
			
			this.VirtualArmor = 80;
			
			this.Tamable = false;
            this.ControlSlots = 4;
            this.MinTameSkill = 95.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Gems, 4 );
			AddLoot( LootPack.Poor, 4 );
			Name = "King Kong";
			Hue = 0;
			SetStr( 1096, 1185 );
			SetDex( 155, 175 );
			SetInt( 686, 775 );

			SetHits( 2500, 3000 );



			Fame = 22500;
			Karma = 22500;
		}


		public KingKong( Serial serial ) : base( serial )
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
