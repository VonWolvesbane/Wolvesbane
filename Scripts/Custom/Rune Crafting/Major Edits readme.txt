The Old Rune crafting system was a very unique idea in my opinion 
con's : for editing success chance or strength of the system required a Whole lotta time going thru each one on 1-6 locations to change succes chance and fail chance / and again for strength on every layer.

Names where cryptic and hard to follow
playing with system was slow having to make a blank rune then a rune 
if this is too strong or low break chance can make players extremely overpowered requiring stronger pvm to fight 
Alot of duplicate code 
Crafting menu was hard to follow 
.
Removed BagOfRunes item 
Redesigned craft menu and sorted by Type of rune/alphabetical *kinda* 
Re-structured code alot of redundacy removed
Goto line 77 and find your succes/fail and Strength bonus on next line each rune 2 lines to change global for that rune.
deleat blank rune need to speed up crafting.
add'ed custom runes 
Renamed All Runes to what they Do instead of Ahm/Corp/Nox etc. easy to Find Via [add Rune *except for RemoveSlayer is just RemoveSlayer.
deleated 2 original runes 1 i felt was unnessary the other i rewrote Durability bonus to work like powder of fortifiying ( repair item by 10 and max dura 10 )
Merged Slayer deeds into craftable slayer runes. !!! Requires Some Unique ingredients like 25 champion skulls or Sand / Oil Cloth ETc...
Fixed many typos in defrunecrafting and False labeled Reagent ingredients requirements. / Many Messages Fixed aswell. 
if your a fan of rune crafting/enchanting you will be a lover of this.
Hued Rune groups seperately as to quickly identifiy wich class of rune it belongs to by color.

Please see SpellChannelingRune.CS  For example around line 77 how to edit Fail chance to suit your server needs.

Credits goto the original authors i just reworked it Original Authors:
 
 Enchanting System : Orbez  @ http://www.runuo.com/community/threads/rune-crafting-enchanting-system.88443/

 the 7 Super Slayer Deeds merged into craftable runes where exerpted from 

 Slayer Deeds : Sham Bam Pow @ http://www.runuo.com/community/threads/svn-slayer-deeds-slayer-removal-deed.487901/

If you like the Rework Give me a thx , Foo Man Choo / Drafted 

Adding the RoughStone to the ML mining system.
go to Scrips/Engines/Harvest/Mining.cs and find:

if ( Core.ML )
            {
                oreAndStone.BonusResources = new BonusHarvestResource[]
                {
                    new BonusHarvestResource( 0, 95.8664, null, null ), //Nothing   //Note: Rounded the below to .0167 instead of 1/6th of a %.  Close enough
                    new BonusHarvestResource( 100, .0167, 1072562, typeof( BlueDiamond ) ),
                    new BonusHarvestResource( 100, .0167, 1072567, typeof( DarkSapphire ) ),
                    new BonusHarvestResource( 100, .0167, 1072570, typeof( EcruCitrine ) ),
                    new BonusHarvestResource( 100, .0167, 1072564, typeof( FireRuby ) ),
                    new BonusHarvestResource( 100, .0167, 1072566, typeof( PerfectEmerald ) ),
                    new BonusHarvestResource( 100, .0167, 1072568, typeof( Turquoise ) )

**and add the new bonus harvest resource:
  
                    new BonusHarvestResource( 100, .5, "you dig up a Rough Stone and put it in your backpack", typeof( RoughStone ))//added for runecrafting

**Your mining.cs code should look like:

if ( Core.ML )
            {
                oreAndStone.BonusResources = new BonusHarvestResource[]
                {
                    new BonusHarvestResource( 0, 98.498, null, null ), //Nothing   //Note: Rounded the below to .0167 instead of 1/6th of a %.  Close enough
                    new BonusHarvestResource( 100, .167, 1072562, typeof( BlueDiamond ) ),
                    new BonusHarvestResource( 100, .167, 1072567, typeof( DarkSapphire ) ),
                    new BonusHarvestResource( 100, .167, 1072570, typeof( EcruCitrine ) ),
                    new BonusHarvestResource( 100, .167, 1072564, typeof( FireRuby ) ),
                    new BonusHarvestResource( 100, .167, 1072566, typeof( PerfectEmerald ) ),
                    new BonusHarvestResource( 100, .167, 1072568, typeof( Turquoise ) ),
                    new BonusHarvestResource( 100, .5, "you dig up a Rough Stone and put it in your backpack", typeof( RoughStone ))//added for runecrafting
 
***************************************************^^^  these ammounts all added up should = 100 ** 98.498 + ( .0167 * 6 ) + .5 = ? if not 100 change 98.498 til its correct. ************************************ this is how you change the rate at wich they may be mined up .5 and 98.498 %chance to get ore

 