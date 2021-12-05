using System;

namespace Server.Items
{
    public class PhoenixRobe : AnniversaryRobe
	{
		public override bool IsArtifact { get { return true; } }
		public override int ArtifactRarity{ get { return 10; } }
        [Constructable]
        public PhoenixRobe()
        {
			Name = "<Body bgcolor=#D87820; text=#F8370E><Big><center>Phoenixs Robes Of Flames [Replica]</Body>";
            Hue = 2736;
        }

        public PhoenixRobe(Serial serial)
            : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();           
        }
    }
}