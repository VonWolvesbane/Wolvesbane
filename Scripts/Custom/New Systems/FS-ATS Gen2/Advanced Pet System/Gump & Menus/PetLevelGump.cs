using System;
using Server;
using Server.Gumps;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.VvV;
using Server.SkillHandlers;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class PetLevelGump : Gump
	{
		private Mobile m_Pet;

		public PetLevelGump( Mobile pet ) : base( 0, 0 )
		{
			m_Pet = pet;

			BaseCreature bc = (BaseCreature)m_Pet;

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;

			AddPage(0);

			AddBackground(12, 9, 650, 526, 2620);
			AddImageTiled(17, 15, 640, 113, 9274);
			AddImageTiled(17, 136, 500, 27, 9274);
			AddImageTiled(17, 171, 500, 356, 9274);
			AddImageTiled(524, 136, 129, 27, 9274);
			AddImageTiled(524, 171, 129, 354, 9274);
			AddAlphaRegion(16, 15, 636, 511);
			AddLabel(22, 20, 1149, @"Ability Points:");
			AddLabel(22, 40, 1149, @"Pets Current Level:");
			AddLabel(22, 60, 1149, @"Pets Maxium Level:");
			AddLabel(22, 80, 1149, @"Pets Gender:");
			AddLabel(22, 100, 1149, @"Pets Name:");

			AddLabel(116, 20, 64, bc.AbilityPoints.ToString() );
			AddLabel(149, 40, 64, bc.Level.ToString() );
			AddLabel(144, 60, 64, bc.MaxLevel.ToString() );

			AddImage(336, 20, 5549);

			AddButton(300, 100, 2117, 2118, 1, GumpButtonType.Page, 1);
			AddButton(320, 100, 2117, 2118, 1, GumpButtonType.Page, 2);
			
			if ( bc.Female == true )
				AddLabel(107, 80, 64, @"Female");
			else
				AddLabel(107, 80, 64, @"Male");
			AddLabel(96, 100, 64, bc.Name.ToString() );

			AddLabel(22, 140, 1149, @"Property Name");
			AddLabel(555, 140, 1149, @"Amount");

			AddPage(1);

			if ( bc.Level != 0 )
			{
				AddLabel(300, 175, 1149, @"Hit Points");
				AddLabel(300, 200, 1149, @"Stamina");
				AddLabel(300, 225, 1149, @"Mana");

				AddLabel(555, 175, 1149, bc.HitsMax.ToString() + "/" + FSATS.NormalHITS.ToString() );
				AddLabel(555, 200, 1149, bc.StamMax.ToString() + "/" + FSATS.NormalSTAM.ToString() );
				AddLabel(555, 225, 1149, bc.ManaMax.ToString() + "/" + FSATS.NormalMANA.ToString() );

				AddButton(24, 175, 4005, 4006, 1, GumpButtonType.Reply, 0);
				AddLabel(44, 175, 1149, @"+1");
				AddButton(78, 175, 4005, 4006, 101, GumpButtonType.Reply, 0);
				AddLabel(98, 175, 1149, @"+10");
				AddButton(142, 175, 4005, 4006, 201, GumpButtonType.Reply, 0);
				AddLabel(162, 175, 1149, @"+25");
				AddButton(206, 175, 4005, 4006, 301, GumpButtonType.Reply, 0);
				AddLabel(226, 175, 1149, @"+100");
				AddButton(24, 200, 4005, 4006, 2, GumpButtonType.Reply, 0);
				AddLabel(44, 200, 1149, @"+1");
				AddButton(78, 200, 4005, 4006, 102, GumpButtonType.Reply, 0);
				AddLabel(98, 200, 1149, @"+10");
				AddButton(142, 200, 4005, 4006, 202, GumpButtonType.Reply, 0);
				AddLabel(162, 200, 1149, @"+25");
				AddButton(206, 200, 4005, 4006, 302, GumpButtonType.Reply, 0);
				AddLabel(226, 200, 1149, @"+100");
				AddButton(24, 225, 4005, 4006, 3, GumpButtonType.Reply, 0);
				AddLabel(44, 225, 1149, @"+1");
				AddButton(78, 225, 4005, 4006, 103, GumpButtonType.Reply, 0);
				AddLabel(98, 225, 1149, @"+10");
				AddButton(142, 225, 4005, 4006, 203, GumpButtonType.Reply, 0);
				AddLabel(162, 225, 1149, @"+25");
				AddButton(206, 225, 4005, 4006, 303, GumpButtonType.Reply, 0);
				AddLabel(226, 225, 1149, @"+100");
			}
			else
			{
				AddLabel(300, 175, 38, @"-Locked-");
				AddLabel(300, 200, 38, @"-Locked-");
				AddLabel(300, 225, 38, @"-Locked-");

				AddLabel(555, 175, 38, @"???");
				AddLabel(555, 200, 38, @"???");
				AddLabel(555, 225, 38, @"???");
			}

			if ( bc.Level >= 10 )
			{
				AddLabel(300, 250, 1149, @"Physical Resistance");
				AddLabel(300, 275, 1149, @"Fire Resistance");
				AddLabel(300, 300, 1149, @"Cold Resistance");
				AddLabel(300, 325, 1149, @"Energy Resistance");
				AddLabel(300, 350, 1149, @"Poison Resistance");

				AddLabel(555, 250, 1149, bc.PhysicalResistance.ToString() + "/" + FSATS.NormalPhys.ToString() );
				AddLabel(555, 275, 1149, bc.FireResistance.ToString() + "/" + FSATS.NormalFire.ToString() );
				AddLabel(555, 300, 1149, bc.ColdResistance.ToString() + "/" + FSATS.NormalCold.ToString() );
				AddLabel(555, 325, 1149, bc.EnergyResistance.ToString() + "/" + FSATS.NormalEnergy.ToString() );
				AddLabel(555, 350, 1149, bc.PoisonResistance.ToString() + "/" + FSATS.NormalPoison.ToString() );

				AddButton(24, 250, 4005, 4006, 4, GumpButtonType.Reply, 0);
				AddLabel(44, 250, 1149, @"+1");
				AddButton(78, 250, 4005, 4006, 104, GumpButtonType.Reply, 0);
				AddLabel(98, 250, 1149, @"+10");
				AddButton(142, 250, 4005, 4006, 204, GumpButtonType.Reply, 0);
				AddLabel(162, 250, 1149, @"+25");
				AddButton(206, 250, 4005, 4006, 304, GumpButtonType.Reply, 0);
				AddLabel(226, 250, 1149, @"+100");
				AddButton(24, 275, 4005, 4006, 5, GumpButtonType.Reply, 0);
				AddLabel(44, 275, 1149, @"+1");
				AddButton(78, 275, 4005, 4006, 105, GumpButtonType.Reply, 0);
				AddLabel(98, 275, 1149, @"+10");
				AddButton(142, 275, 4005, 4006, 205, GumpButtonType.Reply, 0);
				AddLabel(162, 275, 1149, @"+25");
				AddButton(206, 275, 4005, 4006, 305, GumpButtonType.Reply, 0);
				AddLabel(226, 275, 1149, @"+100");
				AddButton(24, 300, 4005, 4006, 6, GumpButtonType.Reply, 0);
				AddLabel(44, 300, 1149, @"+1");
				AddButton(78, 300, 4005, 4006, 106, GumpButtonType.Reply, 0);
				AddLabel(98, 300, 1149, @"+10");
				AddButton(142, 300, 4005, 4006, 206, GumpButtonType.Reply, 0);
				AddLabel(162, 300, 1149, @"+25");
				AddButton(206, 300, 4005, 4006, 306, GumpButtonType.Reply, 0);
				AddLabel(226, 300, 1149, @"+100");
				AddButton(24, 325, 4005, 4006, 7, GumpButtonType.Reply, 0);
				AddLabel(44, 325, 1149, @"+1");
				AddButton(78, 325, 4005, 4006, 107, GumpButtonType.Reply, 0);
				AddLabel(98, 325, 1149, @"+10");
				AddButton(142, 325, 4005, 4006, 207, GumpButtonType.Reply, 0);
				AddLabel(162, 325, 1149, @"+25");
				AddButton(206, 325, 4005, 4006, 307, GumpButtonType.Reply, 0);
				AddLabel(226, 325, 1149, @"+100");
				AddButton(24, 350, 4005, 4006, 8, GumpButtonType.Reply, 0);
				AddLabel(44, 350, 1149, @"+1");
				AddButton(78, 350, 4005, 4006, 108, GumpButtonType.Reply, 0);
				AddLabel(98, 350, 1149, @"+10");
				AddButton(142, 350, 4005, 4006, 208, GumpButtonType.Reply, 0);
				AddLabel(162, 350, 1149, @"+25");
				AddButton(206, 350, 4005, 4006, 308, GumpButtonType.Reply, 0);
				AddLabel(226, 350, 1149, @"+100");
			}
			else
			{
				AddLabel(300, 250, 38, @"-Locked-");
				AddLabel(300, 275, 38, @"-Locked-");
				AddLabel(300, 300, 38, @"-Locked-");
				AddLabel(300, 325, 38, @"-Locked-");
				AddLabel(300, 350, 38, @"-Locked-");

				AddLabel(555, 250, 38, @"???");
				AddLabel(555, 275, 38, @"???");
				AddLabel(555, 300, 38, @"???");
				AddLabel(555, 325, 38, @"???");
				AddLabel(555, 350, 38, @"???");
			}

			if ( bc.Level >= 17 )
			{
				AddLabel(300, 375, 1149, @"Min Damage");
				AddLabel(300, 400, 1149, @"Max Damage");

				AddLabel(555, 375, 1149, bc.DamageMin.ToString() + "/"  + FSATS.NormalMinDam.ToString() );
				AddLabel(555, 400, 1149, bc.DamageMax.ToString() + "/"  + FSATS.NormalMaxDam.ToString() );

				AddButton(24, 375, 4005, 4006, 9, GumpButtonType.Reply, 0);
				AddLabel(44, 375, 1149, @"+1");
				AddButton(78, 375, 4005, 4006, 109, GumpButtonType.Reply, 0);
				AddLabel(98, 375, 1149, @"+10");
				AddButton(142, 375, 4005, 4006, 209, GumpButtonType.Reply, 0);
				AddLabel(162, 375, 1149, @"+25");
				AddButton(206, 375, 4005, 4006, 309, GumpButtonType.Reply, 0);
				AddLabel(226, 375, 1149, @"+100");
				AddButton(24, 400, 4005, 4006, 10, GumpButtonType.Reply, 0);
				AddLabel(44, 400, 1149, @"+1");
				AddButton(78, 400, 4005, 4006, 110, GumpButtonType.Reply, 0);
				AddLabel(98, 400, 1149, @"+10");
				AddButton(142, 400, 4005, 4006, 210, GumpButtonType.Reply, 0);
				AddLabel(162, 400, 1149, @"+25");
				AddButton(206, 400, 4005, 4006, 310, GumpButtonType.Reply, 0);
				AddLabel(226, 400, 1149, @"+100");
			}
			else
			{
				AddLabel(300, 375, 38, @"-Locked-");
				AddLabel(300, 400, 38, @"-Locked-");

				AddLabel(555, 375, 38, @"???");
				AddLabel(555, 400, 38, @"???");
			}
			
			if ( bc.Level >= 20 )
			{
				AddLabel(300, 425, 1149, @"Armor Rating");

				AddLabel(555, 425, 1149, bc.VirtualArmor.ToString() + "/" + FSATS.NormalVArmor.ToString() );

				AddButton(24, 425, 4005, 4006, 11, GumpButtonType.Reply, 0);
				AddLabel(44, 425, 1149, @"+1");
				AddButton(78, 425, 4005, 4006, 111, GumpButtonType.Reply, 0);
				AddLabel(98, 425, 1149, @"+10");
				AddButton(142, 425, 4005, 4006, 211, GumpButtonType.Reply, 0);
				AddLabel(162, 425, 1149, @"+25");
				AddButton(206, 425, 4005, 4006, 311, GumpButtonType.Reply, 0);
				AddLabel(226, 425, 1149, @"+100");
			}
			else
			{
				AddLabel(300, 425, 38, @"-Locked-");

				AddLabel(555, 425, 38, @"???");
			}

			if ( bc.Level >= 40 )
			{
				AddLabel(300, 450, 1149, @"Strength");
				AddLabel(300, 475, 1149, @"Dexterity");
				AddLabel(300, 500, 1149, @"Intelligence");

				AddLabel(555, 450, 1149, bc.RawStr.ToString() + "/" + FSATS.NormalSTR.ToString() );
				AddLabel(555, 475, 1149, bc.RawDex.ToString() + "/" + FSATS.NormalDEX.ToString() );
				AddLabel(555, 500, 1149, bc.RawInt.ToString() + "/" + FSATS.NormalINT.ToString() );

				AddButton(24, 450, 4005, 4006, 12, GumpButtonType.Reply, 0);
				AddLabel(44, 450, 1149, @"+1");
				AddButton(78, 450, 4005, 4006, 112, GumpButtonType.Reply, 0);
				AddLabel(98, 450, 1149, @"+10");
				AddButton(142, 450, 4005, 4006, 212, GumpButtonType.Reply, 0);
				AddLabel(162, 450, 1149, @"+25");
				AddButton(206, 450, 4005, 4006, 312, GumpButtonType.Reply, 0);
				AddLabel(226, 450, 1149, @"+100");
				AddButton(24, 475, 4005, 4006, 13, GumpButtonType.Reply, 0);
				AddLabel(44, 475, 1149, @"+1");
				AddButton(78, 475, 4005, 4006, 113, GumpButtonType.Reply, 0);
				AddLabel(98, 475, 1149, @"+10");
				AddButton(142, 475, 4005, 4006, 213, GumpButtonType.Reply, 0);
				AddLabel(162, 475, 1149, @"+25");
				AddButton(206, 475, 4005, 4006, 313, GumpButtonType.Reply, 0);
				AddLabel(226, 475, 1149, @"+100");
				AddButton(24, 500, 4005, 4006, 14, GumpButtonType.Reply, 0);
				AddLabel(44, 500, 1149, @"+1");
				AddButton(78, 500, 4005, 4006, 114, GumpButtonType.Reply, 0);
				AddLabel(98, 500, 1149, @"+10");
				AddButton(142, 500, 4005, 4006, 214, GumpButtonType.Reply, 0);
				AddLabel(162, 500, 1149, @"+25");
				AddButton(206, 500, 4005, 4006, 314, GumpButtonType.Reply, 0);
				AddLabel(226, 500, 1149, @"+100");
			}
			else
			{
				AddLabel(300, 450, 38, @"-Locked-");
				AddLabel(300, 475, 38, @"-Locked-");
				AddLabel(300, 500, 38, @"-Locked-");

				AddLabel(555, 450, 38, @"???");
				AddLabel(555, 475, 38, @"???");
				AddLabel(555, 500, 38, @"???");
			}

			AddPage(2);

			if ( bc.Level > 35 )
			{
				AddLabel(300, 175, 38, @"100 Ability Points Required per ability");
				
				AddLabel(300, 200, 1149, @"Repel");
				if ( bc.HasAbility(SpecialAbility.Repel))
				{
				AddLabel(555, 200, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 200, 4005, 4006, 15, GumpButtonType.Reply, 0);//extra
				}
				
				AddLabel(300, 225, 1149, @"Poison Attack");
				if ( bc.HasAbility(SpecialAbility.VenomousBite))
				{
				AddLabel(555, 225, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 225, 4005, 4006, 16, GumpButtonType.Reply, 0);//extra
				}
				
				AddLabel(300, 250, 1149, @"Fire Breath Attack");
				if ( bc.HasAbility(SpecialAbility.DragonBreath))
				{
				AddLabel(555, 250, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 250, 4005, 4006, 17, GumpButtonType.Reply, 0);//extra
				}
			}
			else
			{
				AddLabel(300, 175, 38, @"100 Ability Points Required per ability");
				AddLabel(300, 200, 38, @"-Locked-");
				AddLabel(300, 225, 38, @"-Locked-");
				AddLabel(300, 250, 38, @"-Locked-");

				AddLabel(555, 200, 38, @"???");
				AddLabel(555, 225, 38, @"???");
				AddLabel(555, 250, 38, @"???");
				

			}
			if ( bc.Level > 45 )
			{
				AddLabel(300, 300, 38, @"250 Ability Points Required per ability");
				
				AddLabel(300, 325, 1149, @"Heal"); //extra
				if ( bc.HasAbility(SpecialAbility.Heal))
				{
				AddLabel(555, 325, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 325, 4005, 4006, 18, GumpButtonType.Reply, 0);//extra
				}
				
				AddLabel(300, 350, 1149, @"Colossal Rage"); //extra
				if ( bc.HasAbility(SpecialAbility.Rage))
				{
				AddLabel(555, 250, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 350, 4005, 4006, 19, GumpButtonType.Reply, 0);//extra
				}
				
				AddLabel(300, 375, 1149, @"Howl Of Cacophony"); //extra
				if ( bc.HasAbility(SpecialAbility.HowlOfCacophony))
				{
				AddLabel(555, 375, 1149, @"Learned");//extra
				}
				else
				{
				AddButton(24, 375, 4005, 4006, 20, GumpButtonType.Reply, 0);//extra
				}
				
			}
		else
			{
				AddLabel(300, 300, 38, @"250 Ability Points Required per ability");
				AddLabel(300, 325, 38, @"-Locked-");
				AddLabel(300, 350, 38, @"-Locked-");
				AddLabel(300, 375, 38, @"-Locked-");

				AddLabel(555, 325, 38, @"???");
				AddLabel(555, 350, 38, @"???");
				AddLabel(555, 375, 38, @"???");
				

			}
		}


		private static void SpendStatPoints(Mobile from, BaseCreature bc, int statButton, int requested)
		{
			if (from == null || bc == null || requested <= 0)
				return;

			int current = 0;
			int cap = 0;

			switch (statButton)
			{
				case 1: current = bc.HitsMax; cap = FSATS.NormalHITS; break;
				case 2: current = bc.StamMax; cap = FSATS.NormalSTAM; break;
				case 3: current = bc.ManaMax; cap = FSATS.NormalMANA; break;
				case 4: current = bc.PhysicalResistanceSeed; cap = FSATS.NormalPhys; break;
				case 5: current = bc.FireResistSeed; cap = FSATS.NormalFire; break;
				case 6: current = bc.ColdResistSeed; cap = FSATS.NormalCold; break;
				case 7: current = bc.EnergyResistSeed; cap = FSATS.NormalEnergy; break;
				case 8: current = bc.PoisonResistSeed; cap = FSATS.NormalPoison; break;
				case 9: current = bc.DamageMin; cap = FSATS.NormalMinDam; break;
				case 10: current = bc.DamageMax; cap = FSATS.NormalMaxDam; break;
				case 11: current = bc.VirtualArmor; cap = FSATS.NormalVArmor; break;
				case 12: current = bc.RawStr; cap = FSATS.NormalSTR; break;
				case 13: current = bc.RawDex; cap = FSATS.NormalDEX; break;
				case 14: current = bc.RawInt; cap = FSATS.NormalINT; break;
				default: return;
			}

			if (current >= cap)
			{
				from.SendMessage("This cannot gain any farther.");
				from.SendGump(new PetLevelGump(bc));
				return;
			}

			if (bc.AbilityPoints <= 0)
			{
				from.SendMessage("Your pet lacks the ability points to do that.");
				from.SendGump(new PetLevelGump(bc));
				return;
			}

			int amount = Math.Min(requested, Math.Min(bc.AbilityPoints, cap - current));

			if (amount <= 0)
			{
				from.SendGump(new PetLevelGump(bc));
				return;
			}

			bc.AbilityPoints -= amount;

			switch (statButton)
			{
				case 1:
					if (bc.HitsMaxSeed != -1)
						bc.HitsMaxSeed += amount;
					else
						bc.HitsMaxSeed = bc.HitsMax + amount;
					break;
				case 2:
					if (bc.StamMaxSeed != -1)
						bc.StamMaxSeed += amount;
					else
						bc.StamMaxSeed = bc.StamMax + amount;
					break;
				case 3:
					if (bc.ManaMaxSeed != -1)
						bc.ManaMaxSeed += amount;
					else
						bc.ManaMaxSeed = bc.ManaMax + amount;
					break;
				case 4: bc.PhysicalResistanceSeed += amount; break;
				case 5: bc.FireResistSeed += amount; break;
				case 6: bc.ColdResistSeed += amount; break;
				case 7: bc.EnergyResistSeed += amount; break;
				case 8: bc.PoisonResistSeed += amount; break;
				case 9: bc.DamageMin += amount; break;
				case 10: bc.DamageMax += amount; break;
				case 11: bc.VirtualArmor += amount; break;
				case 12: bc.Str += amount; break;
				case 13: bc.Dex += amount; break;
				case 14: bc.Int += amount; break;
			}

			from.SendGump(new PetLevelGump(bc));
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile; 

			BaseCreature bc = (BaseCreature)m_Pet;

			if ( from == null )
				return;

			// Wolvesbane bulk stat buttons:
			// 101-114 = +10, 201-214 = +25, 301-314 = +100.
			int bulkButton = info.ButtonID;
			int bulkAmount = 0;
			int statButton = 0;

			if (bulkButton >= 101 && bulkButton <= 114)
			{
				bulkAmount = 10;
				statButton = bulkButton - 100;
			}
			else if (bulkButton >= 201 && bulkButton <= 214)
			{
				bulkAmount = 25;
				statButton = bulkButton - 200;
			}
			else if (bulkButton >= 301 && bulkButton <= 314)
			{
				bulkAmount = 100;
				statButton = bulkButton - 300;
			}

			if (bulkAmount > 0)
			{
				SpendStatPoints(from, bc, statButton, bulkAmount);
				return;
			}

			if ( info.ButtonID == 1 )
			{
				if ( bc.HitsMax >= FSATS.NormalHITS )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;

					if ( bc.HitsMaxSeed != -1 )
						bc.HitsMaxSeed += 1;
					else
						bc.HitsMaxSeed = bc.HitsMax + 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 2 )
			{
				if ( bc.StamMax >= FSATS.NormalSTAM )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;

					if ( bc.StamMaxSeed != -1 )
						bc.StamMaxSeed += 1;
					else
						bc.StamMaxSeed = bc.StamMax + 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 3 )
			{
				if ( bc.ManaMax >= FSATS.NormalMANA )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
				
					if ( bc.ManaMaxSeed != -1 )
						bc.ManaMaxSeed += 1;
					else
						bc.ManaMaxSeed = bc.ManaMax + 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 4 )
			{
				if ( bc.PhysicalResistanceSeed >= FSATS.NormalPhys )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.PhysicalResistanceSeed += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 5 )
			{
				if ( bc.FireResistSeed >= FSATS.NormalFire )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.FireResistSeed += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 6 )
			{
				if ( bc.ColdResistSeed >= FSATS.NormalCold )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.ColdResistSeed += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 7 )
			{
				if ( bc.EnergyResistSeed >= FSATS.NormalEnergy )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.EnergyResistSeed += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 8 )
			{
				if ( bc.PoisonResistSeed >= FSATS.NormalPoison )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.PoisonResistSeed += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 9 )
			{
				if ( bc.DamageMin >= FSATS.NormalMinDam )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.DamageMin += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 10 )
			{
				if ( bc.DamageMax >= FSATS.NormalMaxDam )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.DamageMax += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 11 )
			{
				if ( bc.VirtualArmor >= FSATS.NormalVArmor )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.VirtualArmor += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 12 )
			{
				if ( bc.RawStr >= FSATS.NormalSTR )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.Str += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 13 )
			{
				if ( bc.RawDex >= FSATS.NormalDEX )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.Dex += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 14 )
			{
				if ( bc.RawInt >= FSATS.NormalINT )
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints != 0 )
				{
					bc.AbilityPoints -= 1;
					bc.Int += 1;

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
				}
			}

			if ( info.ButtonID == 15 )
			{
				if ( bc.HasAbility(SpecialAbility.Repel)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 100 )
				{
				    bc.AbilityPoints -= 100; //points needed 100
					
					bc.SetSpecialAbility(SpecialAbility.Repel);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
			}

			if ( info.ButtonID == 16 )
			{
				if ( bc.HasAbility(SpecialAbility.VenomousBite)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 100 )
				{
				    bc.AbilityPoints -= 100; //points needed 100
					
					bc.SetSpecialAbility(SpecialAbility.VenomousBite);
					bc.SetMagicalAbility(MagicalAbility.Poisoning);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
			}

			if ( info.ButtonID == 17 )
			{
				if ( bc.HasAbility(SpecialAbility.DragonBreath)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 100 )
				{
				    bc.AbilityPoints -= 100; //points needed 100
					
					bc.SetSpecialAbility(SpecialAbility.DragonBreath);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
			}
			//Start Extra skills
			if ( info.ButtonID == 18 )
			{
				if ( bc.HasAbility(SpecialAbility.Heal)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 250 )
				{
				    bc.AbilityPoints -= 250; //points needed 250
					
					bc.SetSpecialAbility(SpecialAbility.Heal);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
			}

			if ( info.ButtonID == 19 )
			{
				if ( bc.HasAbility(SpecialAbility.Rage)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 250 )
				{
				    bc.AbilityPoints -= 250; //points needed 250
					
					bc.SetSpecialAbility(SpecialAbility.Rage);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
			}
			//Test 
			if ( info.ButtonID == 20 )
			{
				if ( bc.HasAbility(SpecialAbility.HowlOfCacophony)) //does it already know the skill?
				{
					from.SendMessage( "This cannot gain any farther." );

					if ( bc.AbilityPoints != 0 )
						from.SendGump( new PetLevelGump( bc ) );
				}
				else if ( bc.AbilityPoints >= 250 )
				{
				    bc.AbilityPoints -= 250; //points needed 250
					
					bc.SetSpecialAbility(SpecialAbility.HowlOfCacophony);
					bc.InvalidateProperties();

                if ( bc.AbilityPoints != 0 ) 	
				from.SendGump( new PetLevelGump( bc ) );

				}
				else
				{
					from.SendMessage( "Your pet lacks the ability points to do that." );
					from.SendGump( new PetLevelGump( bc ) );
				}
				//End test
			}	
		}
	}
}