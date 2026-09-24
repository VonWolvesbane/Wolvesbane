using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: ObsidianWingArms

    public class ObsidianWingArms : GargishPlateArms
    {
        public override int ArtifactRarity { get { return 500; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public ObsidianWingArms() { Name = "Obsidian Wing Arms"; Hue = 1175; Setup(); }
        private void Setup() { WBObsidianPieceStats.Apply(this); PhysicalBonus = 6; ArmorAttributes.MageArmor = 1; ArmorAttributes.SelfRepair = 7; }
        public override void OnAdded(object p) { base.OnAdded(p); WBGargoyleSetHelper.QueueCheck(p as Mobile); }
        public override void OnRemoved(object p) { Mobile m = p as Mobile; base.OnRemoved(p); WBGargoyleSetHelper.QueueCheck(m); }
        public ObsidianWingArms(Serial s) : base(s) { }
        public override void Serialize(GenericWriter w) { base.Serialize(w); w.Write(0); }
        public override void Deserialize(GenericReader r) { base.Deserialize(r); r.ReadInt(); }
    }
}
