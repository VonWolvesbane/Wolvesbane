//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 4/13/2019 1:45:05 PM
//=================================================
using System;
using Server;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "an Sabertooth Tiger corpse" )]
	public class SabertoothTiger : Nightmare
	{
		[Constructable]
		public SabertoothTiger()
		{
			this.Name = "Sabertooth Tiger";
			this.Body = 1416;
			this.BaseSoundID = 0;
			
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
			
			this.Tamable = true;
            this.ControlSlots = 3;
            this.MinTameSkill = 95.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 1 );
			AddLoot( LootPack.Gems, 4 );
			AddLoot( LootPack.Poor, 4 );
		}

		public SabertoothTiger( Serial serial ) : base( serial )
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
