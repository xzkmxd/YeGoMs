using Channel.Opcode;
using ChannelServer.Packet;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelServer.Handler
{
    [PacketHead(RecvOpcode.登陆请求, typeof(RecvOpcode))]
    public class CEnterGameHanlder : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //TODO:游戏登陆(0%)
            int cid = mapleBuffer.read<int>();

            //加载角色信息
            CCharacter mapleCharacter = CMapleCharacter.LoadData(cid,client);
            client.SendDatat(PlayerPakcet.GetCharInfo(mapleCharacter, client));
            Console.WriteLine("角色信息:" + mapleCharacter.Name);

        }
    }
}
