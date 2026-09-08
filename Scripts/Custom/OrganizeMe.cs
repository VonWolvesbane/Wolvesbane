/*
 * Organize Me by Tresdni
 * www.uofreedom.com
 * Instantly organize your backpack with a simple command.
 */

#region References

using System.Collections.Generic;
using System.Linq;
using Server.Factions;
using Server.Items;

#endregion

namespace Server.Commands
{
    public class OrganizeMeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("OrganizeMe", AccessLevel.Player, OrganizeMe_OnCommand);
        }

        private static OrganizePouch FindOrCreatePouch(
            Mobile from,
            Backpack bp,
            string name,
            int hue,
            List<OrganizePouch> createdPouches)
        {
            OrganizePouch pouch = bp.Items
                .OfType<OrganizePouch>()
                .FirstOrDefault(p => p.Name == name);

            if (pouch != null)
            {
                return pouch;
            }

            pouch = new OrganizePouch
            {
                Name = name,
                Hue = hue
            };

            // SAFETY:
            // Check whether the backpack can accept another top-level item
            // BEFORE putting the new organizer pouch into it.  This catches
            // backpack item-count limits (and any other normal container hold
            // restriction) before any player items are moved.
            if (!bp.CheckHold(from, pouch, false, true))
            {
                pouch.Delete();
                return null;
            }

            bp.DropItem(pouch);
            createdPouches.Add(pouch);

            return pouch;
        }

        //This command will not move spellbooks, runebooks, blessed, or insured items.
        [Usage("OrganizeMe")]
        [Description("Organize the items in your backpack into pouches.")]
        private static void OrganizeMe_OnCommand(CommandEventArgs arg)
        {
            Mobile from = arg.Mobile;
            Backpack bp = from.Backpack as Backpack;

            if (@from == null || bp == null)
            {
                return;
            }

            var backpackitems = new List<Item>(bp.Items);
            var subcontaineritems = new List<Item>();

            foreach (BaseContainer item in backpackitems.OfType<BaseContainer>())
            {
                subcontaineritems.AddRange(item.Items);
            }

            backpackitems.AddRange(subcontaineritems);

            // Reuse existing organization pouches whenever possible.
            // Track any pouches created during this run so they can be cleaned
            // up safely if the backpack reaches its item-count limit.
            var createdPouches = new List<OrganizePouch>();

            OrganizePouch weaponpouch = FindOrCreatePouch(from, bp, "Weapons", Utility.RandomMetalHue(), createdPouches);
            OrganizePouch armorpouch = FindOrCreatePouch(from, bp, "Armor", Utility.RandomMetalHue(), createdPouches);
            OrganizePouch clothingpouch = FindOrCreatePouch(from, bp, "Clothing", Utility.RandomBrightHue(), createdPouches);
            OrganizePouch jewelpouch = FindOrCreatePouch(from, bp, "Jewelry", Utility.RandomPinkHue(), createdPouches);
            OrganizePouch potionpouch = FindOrCreatePouch(from, bp, "Potions", Utility.RandomOrangeHue(), createdPouches);
            OrganizePouch currencypouch = FindOrCreatePouch(from, bp, "Currency", Utility.RandomYellowHue(), createdPouches);
            OrganizePouch resourcepouch = FindOrCreatePouch(from, bp, "Resources", Utility.RandomNondyedHue(), createdPouches);
            OrganizePouch toolpouch = FindOrCreatePouch(from, bp, "Tools", Utility.RandomMetalHue(), createdPouches);
            OrganizePouch regspouch = FindOrCreatePouch(from, bp, "Reagents", Utility.RandomGreenHue(), createdPouches);
            OrganizePouch miscpouch = FindOrCreatePouch(from, bp, "Misc", 0, createdPouches);

            if (weaponpouch == null || armorpouch == null || clothingpouch == null ||
                jewelpouch == null || potionpouch == null || currencypouch == null ||
                resourcepouch == null || toolpouch == null || regspouch == null ||
                miscpouch == null)
            {
                // No player items have been moved yet. Remove only the empty
                // pouches that were created during this attempted run.
                foreach (OrganizePouch created in createdPouches)
                {
                    if (created != null && !created.Deleted && created.Items.Count == 0)
                    {
                        created.Delete();
                    }
                }

                from.SendMessage("Please check your backpack item count, as you are currently over the limit");
                return;
            }

            var pouches = new List<OrganizePouch>
            {
                weaponpouch,
                armorpouch,
                clothingpouch,
                jewelpouch,
                potionpouch,
                currencypouch,
                resourcepouch,
                toolpouch,
                regspouch,
                miscpouch
            };

            // Keep the organizer pouches neatly positioned.
            int pouchX = 45;

            foreach (OrganizePouch pouch in pouches)
            {
                pouch.X = pouchX;
                pouch.Y = 65;
                pouchX += 10;
            }

            foreach (
                Item item in
                    backpackitems.Where(
                        item =>
                            item.LootType != LootType.Blessed && !item.Insured && !(item is Runebook) &&
                            !(item is Spellbook) && item.Movable))
            {
                if (item is BaseWeapon)
                {
                    weaponpouch.DropItem(item);
                }
                else if (item is BaseArmor)
                {
                    armorpouch.DropItem(item);
                }
                else if (item is BaseClothing)
                {
                    clothingpouch.DropItem(item);
                }
                else if (item is BaseJewel)
                {
                    jewelpouch.DropItem(item);
                }
                else if (item is BasePotion)
                {
                    potionpouch.DropItem(item);
                }
                else if (item is Gold || item is Silver)
                {
                    currencypouch.DropItem(item);
                }
                else if (item is BaseIngot || item is BaseOre || item is Feather || item is BaseBoard || item is Log || item is BaseLeather ||
                         item is Sand || item is BaseGranite)
                {
                    resourcepouch.DropItem(item);
                }
                else if (item is BaseTool)
                {
                    toolpouch.DropItem(item);
                }
                else if (item is BaseReagent)
                {
                    regspouch.DropItem(item);
                }
                else if (item is OrganizePouch)
                {
                    // Never move organizer pouches into another organizer pouch.
                }
                else
                {
                    miscpouch.DropItem(item);
                }
            }

            // Only remove organization pouches that are truly empty.
            // Never delete a pouch simply because it existed before this run;
            // it may still contain an item that the organizer intentionally
            // skipped (blessed, insured, runebook, spellbook, immovable, etc.).
            var todelete =
                @from.Backpack.Items.OfType<OrganizePouch>()
                    .Where(emptypouch => emptypouch.Items.Count <= 0)
                    .ToList();

            foreach (OrganizePouch packtodelete in todelete)
            {
                packtodelete.Delete();
            }
        }
    }
}