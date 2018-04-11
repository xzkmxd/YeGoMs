using System;
using System.Collections.Generic;
using System.Text;
using ChannelServer.Packet;
using Common.Attribute;
using Common.Client;

namespace ChannelServer.Commands
{
    [CommanAttribute("命令测试", "该命令是用于测试的!")]
    public class GMCommand : CommandExecute
    {
        public int Execute(CMapleClient client, string[] paramArrayOfString)
        {
            Console.WriteLine("玩家:" + client.CharacterInfo.character.Name);
            return -1;
        }
    }

    [Comman("升级", "命令格式:!<升级> 等级")]
    public class GMUpLevel : CommandExecute
    {
        public int Execute(CMapleClient client, string[] paramArrayOfString)
        {
            client.SendDatat(PlayerPakcet.ServerMessage(1, "游戏升级命令!"+int.Parse(paramArrayOfString[0])));
            return 1;
        }
    }
}
