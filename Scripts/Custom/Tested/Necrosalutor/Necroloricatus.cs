using System;
using Server;

using Server.Items;
using System.Collections; // needed for ArrayList
using System.Collections.Generic;
using VitaNex.FX;
using System.Linq;

namespace Server.Mobiles
{
	[CorpseName("Corpse Of The Necroloricatus")]
	public class Necroloricatus : BaseCreature
	{
		public override bool ShowFameTitle { get { return false; } }

		[Constructable]
		public Necroloricatus() : base(AIType.AI_Necro, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Necroloricatus";
			Title = "The Lord of Necromancy";

			Body = 400;
			Hue = 0;
			BaseSoundID = 1072;
			
			SetStr(300, 500);
			SetDex(500, 500);
			SetInt(200, 250);

			SetHits(15000, 35000);

			SetDamage(300, 500);

			SetDamageType(ResistanceType.Physical, 170);
			SetDamageType(ResistanceType.Cold, 170);
			SetDamageType(ResistanceType.Fire, 170);

			SetResistance(ResistanceType.Physical, 95, 98);
			SetResistance(ResistanceType.Energy, 95, 98);
			SetResistance(ResistanceType.Poison, 95, 98);
			SetResistance(ResistanceType.Cold, 95, 98);
			SetResistance(ResistanceType.Fire, 90, 97);

			SetSkill(SkillName.Wrestling, 95.1, 100.0);
			SetSkill(SkillName.Magery, 70.1, 100.0);
			SetSkill(SkillName.Anatomy, 95.1, 100.0);
			SetSkill(SkillName.MagicResist, 95.1, 100.0);
			SetSkill(SkillName.Necromancy, 195.1, 200.0);
			SetSkill(SkillName.Tactics, 95.1, 100.0);
			SetSkill(SkillName.Parry, 95.1, 100.0);
			SetSkill(SkillName.Focus, 95.1, 100.0);
			SetSkill(SkillName.SpiritSpeak, 195.0, 250.0);
			SetSkill(SkillName.DetectHidden, 195.0, 250.0);

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 40;


			AddItem(new HoodedShroudOfShadows(1579));

			NecroloricatusShirt shirt = new NecroloricatusShirt();
			shirt.Movable = false;
			AddItem(shirt);

			NecroloricatusGloves gloves = new NecroloricatusGloves();
			gloves.Movable = false;
			AddItem(gloves);

			NecroloricatusCap head = new NecroloricatusCap();
			head.Movable = false;
			AddItem(head);

			NecroloricatusBoots Boots = new NecroloricatusBoots();
			Boots.Movable = false;
			AddItem(Boots);

			NecroloricatusPants pants = new NecroloricatusPants();
			pants.Movable = false;
			AddItem(pants);


			//NecroloricatusNecklace Gorget = new NecroloricatusNecklace();
			//Gorget.Movable = false;
			//AddItem(Gorget);

			Necroacidus Weapon = new Necroacidus();
			Weapon.Movable = false;
			AddItem(Weapon);
						

			PackGold(400, 600);

			Item hair = new Item(Utility.RandomList(8265));
			hair.Hue = 1153;
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);
			switch (Utility.Random(2))
			{
				case 0: new Nightmare().Rider = this; break;
				case 1: new SkeletalMount().Rider = this; break;
			}

		}



		public override void OnDeath(Container c)
		{
			switch (Utility.Random(30))
			{
				case 0: c.DropItem(new Necroacidus()); break;
				case 1: c.DropItem(new NecroloricatusBoots()); break;
				case 2: c.DropItem(new NecroloricatusCap()); break;
				case 3: c.DropItem(new NecroloricatusGloves()); break;
				case 4: c.DropItem(new NecroloricatusNecklace()); break;
				case 5: c.DropItem(new NecroloricatusPants()); break;
				case 6: c.DropItem(new NecroloricatusShirt()); break;
				
				
			}


			base.OnDeath(c);

		}

		public override bool AlwaysAttackable { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
		}

		public void DoSpecialAbility(Mobile target)
		{

			if (0.01 >= Utility.RandomDouble())
			{
				new FireExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new EnergyExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new SmokeExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new WaterRippleEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new EarthExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new AirExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new PoisonExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new FirePentagramEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new WaterWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new FireWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new EarthWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new AirWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new EnergyWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new PoisonWaveEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnGotMeleeAttack(target);
			}

			if (0.01 >= Utility.RandomDouble())
			{
				new TornadoEffect(target, target.Map, target.Direction, 5, effectHandler: ExplosionDamage).Send();
				base.OnDamagedBySpell(target);
			}
		}

		public override void OnDamagedBySpell(Mobile from)
		{
			base.OnDamagedBySpell(from);

			DoSpecialAbility(from);
		}



		public override void OnGotMeleeAttack(Mobile from)
		{
			base.OnGotMeleeAttack(from);

			DoSpecialAbility(from);
		}

		public virtual void ExplosionDamage(EffectInfo info)
		{
			ArrayList list = new ArrayList();
			Effects.PlaySound(info.Source.Location, info.Map, 777);

			foreach (Mobile m in
				info.Source.Location.GetMobilesInRange(info.Map, 0)
					.Where(m => m != null && !m.Deleted && m.CanBeHarmful(m, false, true)))
			{
				if (m == this || !CanBeHarmful(m))
					continue;

				if (m.Player)
					list.Add(m);
			}
			foreach (Mobile m in list)
			{
				DoHarmful(m);
				int toExplode = Utility.RandomMinMax(30, 40);
				m.Damage(toExplode, this);
			}
		}


		public Necroloricatus(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

		}
	}
}
