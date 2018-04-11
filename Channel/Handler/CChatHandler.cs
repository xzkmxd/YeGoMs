using Channel.Opcode;
using ChannelServer.Packet;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelServer.Handler
{
    [PacketHead(RecvOpcode.游戏聊天, typeof(RecvOpcode))]
    public class CChatMessageHanlde : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //TODO:游戏聊天(50%)
            //聊天消息
            string Text = mapleBuffer.read<string>();
            //管理员命令:!<命令> 参数
            //玩家命令:@<命令> 参数
            //以空格作为分割.分割1:命令头部,分割2:参数(参数以逗号分割.)
            string[] commands = Text.Split(" ");
            if (commands.Length >= 2)
            {
                //识别该说话为命令.
                if (commands[0].StartsWith("!<") && commands[0].EndsWith(">"))
                {
                    if (client.CharacterInfo.character.Gm >= 0)//只有管理员才能使用命令,
                    {
                        //存在该符号
                        string Command = commands[0].Trim().TrimStart("!<".ToCharArray()).TrimEnd(">".ToCharArray());
                        Console.WriteLine("玩家:" + client.CharacterInfo.character.Name + "使用命令:" + Command);
                        Commands.CommandProcessor.Processor.Execute(Command, commands[1].Split(","), client);
                    }
                }
                else if (commands[0].StartsWith("@<") && commands[0].EndsWith(">"))
                {
                    //玩家命令...
                }
            }
            else
            {
                client.SendDatat(PlayerPakcet.GetChatText(client.CharacterInfo.character.Id, Text));

            }


        }
    }
}
