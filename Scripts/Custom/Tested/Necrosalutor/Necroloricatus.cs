using Server.Items;

using VitaNex.FX;

namespace Server.Mobiles
{
	[CorpseName("Corpse Of The Necroloricatus")]
	public class Necroloricatus : BaseCreature
	{
		public override bool ShowFameTitle => false;

		public override bool AlwaysAttackable => true;
		public override bool AlwaysMurderer => true;

		[Constructable]
		public Necroloricatus() : base(AIType.AI_Necro, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Necroloricatus";
			Title = "The Lord of Necromancy";

			Body = 400;
			Hue = 0;

			BaseSoundID = 1072;

			HairItemID = 8265;

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 40;

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

			AddItem(new HoodedShroudOfShadows(1579));

			AddItem(new NecroloricatusShirt
			{
				Movable = false
			});

			AddItem(new NecroloricatusGloves
			{
				Movable = false
			});

			AddItem(new NecroloricatusCap
			{
				Movable = false
			});

			AddItem(new NecroloricatusBoots
			{
				Movable = false
			});

			AddItem(new NecroloricatusPants
			{
				Movable = false
			});
			/*
			AddItem(new NecroloricatusNecklace
			{
				Movable = false
			});
			*/
			AddItem(new Necroacidus
			{
				Movable = false
			});

			switch (Utility.Random(2))
			{
				case 0: new Nightmare().Rider = this; break;
				case 1: new SkeletalMount().Rider = this; break;
			}

			PackGold(400, 600);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (c?.Deleted != false)
			{
				return;
			}

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

		public void DoSpecialAbility(Mobile target)
		{
			if (Utility.RandomDouble() <= 0.01)
			{
				switch (Utility.Random(15))
				{
					case 0: new FireExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 1: new EnergyExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 2: new SmokeExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 3: new WaterRippleEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 4: new EarthExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 5: new AirExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 6: new PoisonExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 7: new FirePentagramEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
					case 8: new WaterWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 9: new FireWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 10: new EarthWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 11: new AirWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 12: new EnergyWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 13: new PoisonWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
					case 14: new TornadoEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
				}
			}
		}

		public void ExplosionDamage(EffectInfo info)
		{
			Effects.PlaySound(info.Source, info.Map, 777);

			foreach (var m in info.Source.FindPlayersInRange(info.Map, 0))
			{
				if (CanBeHarmful(m, false, true))
				{
					DoHarmful(m);

					_ = m.Damage(Utility.RandomMinMax(30, 40), this);
				}
			}
		}

		public Necroloricatus(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			_ = reader.ReadInt();
		}
	}
}
