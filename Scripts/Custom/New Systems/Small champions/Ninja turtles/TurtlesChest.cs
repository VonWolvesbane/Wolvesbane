// Created by ReApEr
using System;
using Server;

namespace Server.Items
{
              public class TurtlesChest: Robe
{
              
              [Constructable]
              public TurtlesChest() 
{

                Hue = 53;
              	ItemID = 11111;
                  }
              public TurtlesChest( Serial serial ) : base( serial )
                      {
                      }
              
              public override void Serialize( GenericWriter writer )
                      {
                          base.Serialize( writer );
                          writer.Write( (int) 0 );
                      }
              
              public override void Deserialize(GenericReader reader)
                      {
                          base.Deserialize( reader );
                          int version = reader.ReadInt();
                      }
                  }
              }
