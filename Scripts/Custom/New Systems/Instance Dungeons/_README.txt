

	Instance Maps is a Vita-Nex: Core Service and does not require manual activation.
	You can configure some options of the service by finding Instance Maps in the [VNC control panel and clicking [CONFIG].


	Dungeon Portals can be added with [ADD DungeonPortal
	Once added, a Dungeon Portal can be configured by using [PROPS and targeting it.
	In order to link the portal to a specific Dungeon, you must change the ID property.
	The ID property will automatically update the gate with all relevant information for the Dungeon you selected.
	If your selection is invalid, or the Dungeon has issues, the portal may refuse to link it, or refuse entry when used.


	[DungeonGen
	Opens an interface that allows you to generate a new Dungeon script.

	Some fields, such as the Name, are required.
	The Name must not contain invalid characters, stick to A-Z, 0-9.
	Always ensure the first character of the Name is a letter, not a number.

	When selecting the export area, you need to include the entire dungeon area that contains deco to export.
	AddOns and Multis are also exported as decorative components.
	If there are too many components to generate code for, 
	the export will generate a text-based config file that contains the list of components that the new Dungeon will load.


	The exported Dungeon file will be bare-bones, 
	but contain enough information for you to edit and test it easily.

	The new Dungeon will need to be manually registered.
	You will need to add a new ID for your Dungeon to the DungeonID enum, 
	then make sure your new Dungeon's ID property returns your new ID.
	The DungeonID enum can be found in /Dungeons/Core/Objects/DungeonID.cs


	The included "Sewers of Britain" Dungeon serves as a prime example of how to create a dungeon.
	The first few steps of developing the Dungeon's mechanics are typically done in a Dungeon.OnGenerate() method override;

	protected override void OnGenerate()
	{
		base.OnGenerate();

		CreateZone("My Dungeon Region", new Rectangle2D(0, 0, 100, 100)); // Bounds are real map points

		// Spawning a Mobile, Item, or Static can be done with the following methods or their overloads;
		//
		// Mobile	<-	CreateMobile(Type type, Point3D p, bool replacePack, bool scale, double factor, object[] args)
		// Item		<-	CreateItem(Type type, Point3D p, bool scale, double factor, object[] args)
		// Static	<-	CreateStatic(int itemID, Point3D p, bool checkExist)
		//
		// There are also tile versions of some of these methods for creating sections of objects.
	}