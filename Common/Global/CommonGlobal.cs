using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Global
{
    public class CommonGlobal
    {
        public static Dictionary<short, HandlerInterface> mHandler = new Dictionary<short, HandlerInterface>();

        public static void Run(short id,MapleBuffer mapleBuffer,MapleClient client)
        {
            if (mHandler.ContainsKey(id))
            {
                object[] attributes = mHandler[id].GetType().GetCustomAttributes(typeof(PacketHead),true);//.Attributes;
                foreach(object packethead in attributes )
                {
                    if(packethead is PacketHead) {
                        Console.WriteLine("运行:" + ((PacketHead)packethead).Text);                        
                    }
                }
                mHandler[id].Handle(mapleBuffer, client);
            }
        }

    }
}
