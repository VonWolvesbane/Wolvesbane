using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: AeternumStoneLegs

    public class AeternumStoneLegs : GargishPlateLegs
    {
        public override int ArtifactRarity { get { return 777; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public AeternumStoneLegs() { Name = "Aeternum Stone Leggings"; Hue = 2406; Setup(); }
        private void Setup() { WBAeternumPieceStats.Apply(this); PhysicalBonus = 10; ArmorAttributes.MageArmor = 1; ArmorAttributes.SelfRepair = 10; }
        public override void OnAdded(object p) { base.OnAdded(p); WBGargoyleSetHelper.QueueCheck(p as Mobile); }
        public override void OnRemoved(object p) { Mobile m = p as Mobile; base.OnRemoved(p); WBGargoyleSetHelper.QueueCheck(m); }
        public AeternumStoneLegs(Serial s) : base(s) { }
        public override void Serialize(GenericWriter w) { base.Serialize(w); w.Write(0); }
        public override void Deserialize(GenericReader r) { base.Deserialize(r); r.ReadInt(); }
    }
}
