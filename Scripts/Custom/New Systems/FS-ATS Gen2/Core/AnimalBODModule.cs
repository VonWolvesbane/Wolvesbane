using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Engines.BulkOrders;
using System.Collections.Generic;

namespace CustomsFramework.Systems.AnimalBODSystem
{
    public class AnimalBODModule : BaseModule
    { 
        private Server.Engines.BulkOrders.TamingBOBFilter m_TamingBOBFilter;
        public Server.Engines.BulkOrders.TamingBOBFilter TamingBOBFilter { get { return m_TamingBOBFilter; } }
        
        private DateTime m_NextTamingBulkOrder;
       	public TimeSpan NextTamingBulkOrder
		{
			get
			{
				TimeSpan ts = m_NextTamingBulkOrder - DateTime.UtcNow;

				if ( ts < TimeSpan.Zero )
					ts = TimeSpan.Zero;

				return ts;
			}
			set
			{
				try
                {
                    // Wolvesbane alignment: the legacy Animal BOD vendor may ask
                    // this module for a 60-minute cooldown. Normalize every
                    // positive delay shorter than the shard-wide BOD cadence
                    // to six hours. TimeSpan.Zero remains an immediate/admin reset.
                    if ( value > TimeSpan.Zero && value < TimeSpan.FromHours( 6.0 ) )
                        value = TimeSpan.FromHours( 6.0 );

                    m_NextTamingBulkOrder = DateTime.UtcNow + value;
                }
                catch{}}
		}
        
        public AnimalBODModule(Mobile from) : base()
        {
            this.LinkMobile(from);
            
            m_TamingBOBFilter = new Server.Engines.BulkOrders.TamingBOBFilter();

            BaseCore.OnEnabledChanged += BaseCore_OnEnabledChanged;
        }

        public AnimalBODModule(CustomSerial serial) : base(serial)
        {
            BaseCore.OnEnabledChanged += BaseCore_OnEnabledChanged;
        }
        
        private void BaseCore_OnEnabledChanged(BaseCoreEventArgs e)
        {
        }

        public override String Name
        {
            get
            {
                if (this.LinkedMobile != null)
                    return String.Format("Vanity Pets Module - {0}", this.LinkedMobile.Name);
                else
                    return "Unlinked Vanity Pets Module";
            }
        }

        public override String Description
        {
            get
            {
                if (this.LinkedMobile != null)
                    return String.Format("Vanity Pets Module that is linked to {0}", this.LinkedMobile.Name);
                else
                    return "Unlinked Vanity Pets Module";
            }
        }

        public override String Version { get { return AnimalBODCore.SystemVersion; } }
        public override AccessLevel EditLevel { get { return AccessLevel.Developer; } }
        public override Gump SettingsGump { get { return base.SettingsGump; } }

        public override void Prep()
        {
            base.Prep();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            Utilities.WriteVersion(writer, 1);
            
            m_TamingBOBFilter.Serialize( writer );
            writer.Write( m_NextTamingBulkOrder );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Int32 version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_TamingBOBFilter = new Server.Engines.BulkOrders.TamingBOBFilter( reader );
                    m_NextTamingBulkOrder = reader.ReadDateTime();
                    break;

                case 0:
                    m_TamingBOBFilter = new Server.Engines.BulkOrders.TamingBOBFilter( reader );
                    // Version 0 never persisted this timer.
                    m_NextTamingBulkOrder = DateTime.MinValue;
                    break;
            }
            
            if ( m_TamingBOBFilter == null )
				m_TamingBOBFilter = new Server.Engines.BulkOrders.TamingBOBFilter();
        }
    }
}
