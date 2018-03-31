using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Attribute
{
    public class MaplePacket
    {
        public short Head { get; set; }
        public byte[] Data { get; set; }

        public MaplePacket(short head, byte[] data)
        {
            Head = head;
            Data = data;
        }

    }
}
