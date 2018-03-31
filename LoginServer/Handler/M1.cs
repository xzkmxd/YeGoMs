using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Handler
{
    [PacketHead(6, "测试函数")]
    public class M1 : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
        {
            throw new NotImplementedException();
        }

        public MaplePacket TextGame()
        {
            using (OldMapleBuffer mapleBuffer = new OldMapleBuffer())
            {                
                mapleBuffer.add<int>(5);
                return new MaplePacket((short)5, mapleBuffer.ToArry());
            }
        }
    }
}
