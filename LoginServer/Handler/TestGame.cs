using Common.Buffer;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Attribute;
using Common.Client;

namespace LoginServer.Handler
{
    
    [PacketHead(5, "注册函数")]
    public class TestGame : HandlerInterface
    {

        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
        {            
                int game = mapleBuffer.read<int>();
                int gamew = mapleBuffer.read<int>();
                int aqwe = mapleBuffer.read<int>();
                Console.WriteLine("读取数据555:{0},{1}", game, gamew);
                client.m_Session.WriteAndFlushAsync(TextGame());
        }

        public void SendData(OldMapleBuffer mapleBuffer)
        {
            using (OldMapleBuffer s = mapleBuffer)
            {
                //发送数据
                //
            }
        }

        public MapleBuffer TextGame()
        {
            MapleBuffer mapleBuffer = new MapleBuffer();
            mapleBuffer.add<int>(5);
            return mapleBuffer;
        }
    }
}
