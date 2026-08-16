/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;

namespace daat99
{
	public class Daat99OWLTR : Item 
	{
		public bool Deletable = true;
		public override bool Decays { get { return Deletable; } }
		
		public static int MajorVersion 	=  3;
		public static int MinorVersion 	= 01;
		public static int BuildNumber 	= 00; 
		
		private static Hashtable htStaticHolders = new Hashtable();
		public static Hashtable StaticHolders{ get{ return htStaticHolders; } set{ htStaticHolders = value; } }

		private static Hashtable htTempHolders = new Hashtable();
		public static Hashtable TempHolders{ get{ return htTempHolders; } set{ htTempHolders = value; } }


		public override void Consume() { if (Deletable) base.Consume(); }
		public override void Consume(int amount) { if (Deletable) base.Consume(amount); }
		public override void RemoveItem(Item item) { if (Deletable) base.RemoveItem(item); }
		public override void OnRemoved(object parent) { if (Deletable) base.OnRemoved(parent); }
		public override void OnDelete() { if (Deletable) base.OnDelete(); }
		public override void Delete() { if (Deletable) base.Delete(); }

		[Constructable]
		public Daat99OWLTR() : this(true) { }
		[Constructable] 
		public Daat99OWLTR(bool deletableVisible) : base( 10900 ) 
		{ 
			Hue = 2; 
			Name = "Daat99's OWLTR Control and Information Center";
			Movable = false;
			Light = LightType.Circle300;
			Deletable = deletableVisible;
			Visible = deletableVisible;
		} 

		public override void OnDoubleClick( Mobile from ) 
		{ 
			from.CloseGump( typeof( Daat99CustomOWLTRGump ) );
			if ( !from.Player ) 
				return; 
			if (from.AccessLevel >= AccessLevel.Administrator)
				from.SendGump( new Daat99CustomOWLTRGump((PlayerMobile)from) );
			else
				from.SendGump( new Daat99CustomOWLTRGump((PlayerMobile)from) );
		}

		public static void Initialize()
		{
			OWLTROptionsManager.Manager.InitOwltrOptions();
			bool found = false;
			foreach (Item item in World.Items.Values)
				if (item is Daat99OWLTR && !((Daat99OWLTR)item).Deletable)
					found = true;
			if ( !found )
				GenOWLTR();
			if ( StaticHolders == null )
				StaticHolders = new Hashtable();
			if ( TempHolders == null )
				TempHolders = new Hashtable();

			CommandSystem.Register( "OWLTR", AccessLevel.Player, new CommandEventHandler( OWLTR_OnCommand ) );
			if ( Core.AOS )
				CommandSystem.Register( "OWLTRBOD", AccessLevel.Player, new CommandEventHandler( OWLTRBOD_OnCommand ) );

			if ( OWLTROptionsManager.IsEnabled(OWLTROptionsManager.OPTIONS_ENUM.RECIPE_CRAFT) )
				CommandSystem.Register( "MissingRecipes", AccessLevel.Player, new CommandEventHandler( MissingRecipes_OnCommand ) );
			CommandSystem.Register( "Daat99Holder", AccessLevel.Administrator, new CommandEventHandler( Daat99HolderOnCommand ) );

			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
		}

		[Usage( "OWLTR" )]
		[Description( "Open the OWLTR Control Center Gump." )]
		public static void OWLTR_OnCommand( CommandEventArgs e )
		{
			if (!(e.Mobile is PlayerMobile))
				return;
			if (e.Mobile.AccessLevel >= AccessLevel.Administrator)
				e.Mobile.SendGump(new Daat99CustomOWLTRGump((PlayerMobile)e.Mobile));
			else
				e.Mobile.SendGump(new Daat99CustomOWLTRGump((PlayerMobile)e.Mobile));
		}
		
		[Usage( "OWLTRBOD" )]
		[Description( "Open the bods request Gump." )]
		public static void OWLTRBOD_OnCommand( CommandEventArgs e )
		{
			if (e.Mobile is PlayerMobile)
				e.Mobile.SendGump(new OWLTRbodGump((PlayerMobile)e.Mobile));
		}

		[Usage( "MissingRecipes" )]
		[Description( "Show the player what recipes he's missing." )]
		public static void MissingRecipes_OnCommand( CommandEventArgs e )
		{
			if (e.Mobile is PlayerMobile)
				e.Mobile.SendGump(new MissingRecipesGump((PlayerMobile)e.Mobile, 0));
		}
		
		[Usage( "Daat99Holder" )]
		[Description( "Send the administrator the Daat99Holder gump of a specific player." )]
		public static void Daat99HolderOnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new Daat99Target(0);
		}
			
		public static void GenOWLTR()
		{
			Daat99OWLTR dowl = new Daat99OWLTR();
			dowl.MoveToWorld( new Point3D(1434,1707,18), Map.Trammel );
			Daat99OWLTR dowl2 = new Daat99OWLTR();
			dowl2.MoveToWorld( new Point3D(1434,1707,18), Map.Felucca );
			Daat99OWLTR dowl3 = new Daat99OWLTR(false);
			dowl3.MoveToWorld( new Point3D(0,0,0), Map.Tokuno );
		}

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;
			
			if ( !htTempHolders.Contains(from) )
			{
				if ( !htStaticHolders.Contains(from) )
					htStaticHolders.Add( from, new NewDaat99Holder() );
				htTempHolders.Add( from, htStaticHolders[from] );
			}
		}
			
		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
			htStaticHolders[from] = htTempHolders[from];
			htTempHolders.Remove( from );
		}

		public Daat99OWLTR( Serial serial ) : base( serial ) 
		{ 
		} 

		// Wolvesbane Phase 15:
		// Cache the shared recipe catalog between saves. The save format itself is unchanged.
		// The cache is validated against every holder before anything is written. If a new
		// recipe Type appears, the catalog is rebuilt in stable sorted order first.
		private static ArrayList wbRecipeCatalogCache;
		private static Hashtable wbRecipeTypeIndexCache;

		private class WBPreparedHolder
		{
			public Mobile Mobile;
			public NewDaat99Holder Holder;
			public int[] Words;

			public WBPreparedHolder(Mobile mobile, NewDaat99Holder holder, int[] words)
			{
				Mobile = mobile;
				Holder = holder;
				Words = words;
			}
		}

		private static void WBRebuildRecipeCatalog( Hashtable serialTable )
		{
			Hashtable uniqueByName = new Hashtable();
			ArrayList catalog = new ArrayList();

			foreach ( DictionaryEntry de in serialTable )
			{
				NewDaat99Holder holder = de.Value as NewDaat99Holder;
				if ( holder == null || holder.ItemTypeList == null )
					continue;

				for ( int i = 0; i < holder.ItemTypeList.Count; ++i )
				{
					Type type = holder.ItemTypeList[i] as Type;
					if ( type == null )
						continue;

					string name = type.ToString();
					if ( name == null || uniqueByName.Contains(name) )
						continue;

					uniqueByName.Add(name, type);
					catalog.Add(name);
				}
			}

			catalog.Sort();

			Hashtable typeIndex = new Hashtable();

			for ( int i = 0; i < catalog.Count; ++i )
			{
				string name = catalog[i] as string;
				Type type = uniqueByName[name] as Type;

				if ( type != null )
					typeIndex[type] = i;
			}

			wbRecipeCatalogCache = catalog;
			wbRecipeTypeIndexCache = typeIndex;
		}

		private static bool WBRecipeCatalogNeedsRebuild( Hashtable serialTable )
		{
			if ( wbRecipeCatalogCache == null || wbRecipeTypeIndexCache == null )
				return true;

			foreach ( DictionaryEntry de in serialTable )
			{
				NewDaat99Holder holder = de.Value as NewDaat99Holder;
				if ( holder == null || holder.ItemTypeList == null )
					continue;

				for ( int i = 0; i < holder.ItemTypeList.Count; ++i )
				{
					Type type = holder.ItemTypeList[i] as Type;
					if ( type != null && !wbRecipeTypeIndexCache.Contains(type) )
						return true;
				}
			}

			return false;
		}

		private static ArrayList WBPrepareHolders( Hashtable serialTable, Hashtable typeIndex, int catalogCount )
		{
			ArrayList prepared = new ArrayList();
			int wordCount = (catalogCount + 31) / 32;

			foreach ( DictionaryEntry de in serialTable )
			{
				Mobile mobile = de.Key as Mobile;
				NewDaat99Holder holder = de.Value as NewDaat99Holder;

				if ( mobile == null || holder == null )
					continue;

				int[] words = new int[wordCount];
				ArrayList types = holder.ItemTypeList;

				if ( types != null )
				{
					for ( int i = 0; i < types.Count; ++i )
					{
						Type type = types[i] as Type;
						if ( type == null )
							continue;

						object indexObject = typeIndex[type];
						if ( indexObject == null )
							continue;

						int index = (int)indexObject;

						if ( index >= 0 && index < catalogCount )
							words[index >> 5] |= (1 << (index & 31));
					}
				}

				prepared.Add(new WBPreparedHolder(mobile, holder, words));
			}

			return prepared;
		}

		private static Hashtable BuildSerializableHolderTable()
		{
			Hashtable serialTable = new Hashtable();

			foreach ( DictionaryEntry de in htStaticHolders )
			{
				Mobile mobile = de.Key as Mobile;
				NewDaat99Holder holder = de.Value as NewDaat99Holder;

				if ( mobile != null && holder != null && !serialTable.Contains(mobile) )
					serialTable.Add( mobile, holder );
			}

			return serialTable;
		}

		private static ArrayList BuildRecipeCatalog( Hashtable serialTable, Hashtable typeIndex )
		{
			Hashtable names = new Hashtable();
			ArrayList catalog = new ArrayList();

			foreach ( DictionaryEntry de in serialTable )
			{
				NewDaat99Holder holder = de.Value as NewDaat99Holder;
				if ( holder == null || holder.ItemTypeList == null )
					continue;

				for ( int i = 0; i < holder.ItemTypeList.Count; ++i )
				{
					Type type = holder.ItemTypeList[i] as Type;
					if ( type == null )
						continue;

					string name = type.ToString();
					if ( name == null || names.Contains(name) )
						continue;

					names.Add(name, type);
					catalog.Add(name);
				}
			}

			// Stable ordering makes saves reproducible and keeps each holder's bit positions
			// tied to the catalog written immediately before the holder table.
			catalog.Sort();

			for ( int i = 0; i < catalog.Count; ++i )
				typeIndex[catalog[i]] = i;

			return catalog;
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			Stopwatch total = Stopwatch.StartNew();
			Stopwatch sw = Stopwatch.StartNew();

			base.Serialize( writer ); 
			double baseMs = sw.Elapsed.TotalMilliseconds;

			sw.Restart();
			writer.Write( (int) 1 ); // Wolvesbane compact OWLTR serialization
			writer.Write( (bool) Deletable ); //must be written first
			double headerMs = sw.Elapsed.TotalMilliseconds;

			if (!Deletable)
			{
				sw.Restart();
				OWLTROptionsManager.Manager.Serialize(writer);
				double optionsMs = sw.Elapsed.TotalMilliseconds;

				sw.Restart();
				int tempSeen = 0;
				int tempUpdated = 0;
				foreach (DictionaryEntry de in htTempHolders) //update the static hashtable before save
				{
					tempSeen++;
					Mobile mobile = de.Key as Mobile;
					if ( mobile != null && htStaticHolders.Contains(mobile) )
					{
						htStaticHolders[mobile] = de.Value;
						tempUpdated++;
					}
				}
				double tempSyncMs = sw.Elapsed.TotalMilliseconds;

				sw.Restart();
				Hashtable serialTable = BuildSerializableHolderTable();
				double serialTableMs = sw.Elapsed.TotalMilliseconds;

				sw.Restart();
				bool catalogRebuilt = WBRecipeCatalogNeedsRebuild(serialTable);

				if ( catalogRebuilt )
					WBRebuildRecipeCatalog(serialTable);

				ArrayList recipeCatalog = wbRecipeCatalogCache;
				Hashtable typeIndex = wbRecipeTypeIndexCache;
				double catalogBuildMs = sw.Elapsed.TotalMilliseconds;

				// Build each holder bitset once, before the catalog/holders are written.
				// This replaces the old second pass that converted every Type to a string
				// and performed a string-keyed lookup during SerializeCompact().
				sw.Restart();
				ArrayList preparedHolders = WBPrepareHolders(serialTable, typeIndex, recipeCatalog.Count);
				double holderPrepareMs = sw.Elapsed.TotalMilliseconds;

				sw.Restart();
				writer.Write( recipeCatalog.Count );
				long catalogChars = 0;
				for ( int i = 0; i < recipeCatalog.Count; ++i )
				{
					string name = (string)recipeCatalog[i];
					if (name != null) catalogChars += name.Length;
					writer.Write( name );
				}
				double catalogWriteMs = sw.Elapsed.TotalMilliseconds;

				NewDaat99Holder.WBResetSerializeProfile();
				sw.Restart();
				writer.Write( preparedHolders.Count );
				double slowestHolderMs = 0.0;
				Serial slowestHolderSerial = Serial.MinusOne;

				for ( int i = 0; i < preparedHolders.Count; ++i )
				{
					WBPreparedHolder prepared = preparedHolders[i];
					writer.Write( prepared.Mobile );

					long holderStart = Stopwatch.GetTimestamp();
					prepared.Holder.SerializeCompactPrepared( writer, prepared.Words );
					double holderMs = (Stopwatch.GetTimestamp() - holderStart) * 1000.0 / Stopwatch.Frequency;

					if (holderMs > slowestHolderMs)
					{
						slowestHolderMs = holderMs;
						slowestHolderSerial = prepared.Mobile != null ? prepared.Mobile.Serial : Serial.MinusOne;
					}
				}
				double holdersMs = sw.Elapsed.TotalMilliseconds;

				total.Stop();

				Console.WriteLine(
					"WB OWLTR PROFILE: total={0:0.000}ms base={1:0.000} header={2:0.000} options={3:0.000} tempSync={4:0.000} serialTable={5:0.000} catalogCheck/Rebuild={6:0.000} holderPrepare={7:0.000} catalogWrite={8:0.000} holdersWrite={9:0.000}",
					total.Elapsed.TotalMilliseconds, baseMs, headerMs, optionsMs, tempSyncMs, serialTableMs, catalogBuildMs, holderPrepareMs, catalogWriteMs, holdersMs);

				Console.WriteLine(
					"WB OWLTR COUNTS: static={0:N0} temp={1:N0} tempSeen={2:N0} tempUpdated={3:N0} serializable={4:N0} prepared={5:N0} catalog={6:N0} catalogChars={7:N0} rebuilt={8} slowestHolder={9} {10:0.000}ms",
					htStaticHolders != null ? htStaticHolders.Count : 0,
					htTempHolders != null ? htTempHolders.Count : 0,
					tempSeen, tempUpdated, serialTable.Count, preparedHolders.Count, recipeCatalog.Count, catalogChars,
					catalogRebuilt ? "yes" : "no", slowestHolderSerial, slowestHolderMs);

				Console.WriteLine("WB OWLTR HOLDERS: " + NewDaat99Holder.WBGetSerializeProfile());
			}
			else
			{
				total.Stop();
			}
		}

		public override void Deserialize( GenericReader reader ) 
		{
			base.Deserialize( reader ); 

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
				{
					Deletable = reader.ReadBool();

					if (!Deletable)
					{
						OWLTROptionsManager.Manager.Deserialize(reader);

						int catalogCount = reader.ReadInt();
						Type[] recipeCatalog = new Type[catalogCount];

						for ( int i = 0; i < catalogCount; ++i )
						{
							string typeName = reader.ReadString();
							try { recipeCatalog[i] = ScriptCompiler.FindTypeByFullName(typeName); }
							catch { recipeCatalog[i] = null; }
						}

						int count = reader.ReadInt();
						for ( int i = 0; i < count; ++i )
						{
							Mobile from = reader.ReadMobile();
							NewDaat99Holder ndh = new NewDaat99Holder( reader, recipeCatalog );

							if ( from != null )
								htStaticHolders[from] = ndh;
						}
					}
					break;
				}

				case 0:
				{
					// Full backward compatibility with every existing Wolvesbane OWLTR save.
					Deletable = reader.ReadBool();

					if (!Deletable)
					{
						OWLTROptionsManager.Manager.Deserialize(reader);

						int count = reader.ReadInt();
						for ( int i = 0; i < count; ++i )
						{
							Mobile from = reader.ReadMobile();
							NewDaat99Holder ndh = new NewDaat99Holder( reader );

							if ( from != null )
								htStaticHolders[from] = ndh;
						}
					}
					break;
				}

				default:
					throw new Exception("Unsupported Daat99OWLTR serialization version: " + version);
			}
		}

		public class OwltrOps
		{
			private string s_Title;
			private bool b_Setting;
			private string s_Desctiption;
			public string Title{ get{ return s_Title;} set{ s_Title = value;} }
			public bool Setting{ get{ return b_Setting;} set{ b_Setting= value;} }
			public string Desctiption{ get{ return s_Desctiption;} set{ s_Desctiption= value;} }
			
			public OwltrOps(string name, bool setting, string desc)
			{
				s_Title = name;
				b_Setting = setting;
				s_Desctiption = desc;
			}
		}

		private class Daat99Target : Target
		{
			private int iSwitch;
 
			public Daat99Target(int i) : base(12, false, TargetFlags.None)
			{
				iSwitch = i;
			}
 
			protected override void OnTarget(Mobile from, object target)
			{
				switch (iSwitch)
				{
					case 0: //daat99holder command
					{
						if (target is PlayerMobile)
							from.SendGump( new Daat99HolderGump( from, (NewDaat99Holder)htTempHolders[(Mobile)target], (Mobile)target, true ) );
						else
							from.SendMessage("You must target a player.");
						break;
					}
				}
			}
		}
	}
}