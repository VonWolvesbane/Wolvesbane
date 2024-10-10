using System;
using Server;
using Server.Items;

[Flipable(0xA2CA, 0xA2CB)]
public class InvisRobeSkin : BaseClothing
{
    [Constructable]
    public InvisRobeSkin() 
        : base(0xA2CA, Layer.OuterTorso)
    {
        Name = "Hide Robe Skin";
        Weight = 1.0;
        Hue = 0;
    }

    public InvisRobeSkin(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        reader.ReadInt();
    }
}
