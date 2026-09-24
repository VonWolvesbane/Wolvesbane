using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: ObsidianWingArmor

    public class ObsidianWingArmor : GargishLeatherWingArmor
    {
        public override int ArtifactRarity { get { return 500; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public ObsidianWingArmor() { Name = "Obsidian Wing Armor"; Hue = 1175; WBObsidianPieceStats.Apply(this); }
        public override void OnAdded(object p) { base.OnAdded(p); WBGargoyleSetHelper.QueueCheck(p as Mobile); }
        public override void OnRemoved(object p) { Mobile m = p as Mobile; base.OnRemoved(p); WBGargoyleSetHelper.QueueCheck(m); }
        public ObsidianWingArmor(Serial s) : base(s) { }
        public override void Serialize(GenericWriter w) { base.Serialize(w); w.Write(0); }
        public override void Deserialize(GenericReader r) { base.Deserialize(r); r.ReadInt(); }
    }
}
