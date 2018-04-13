using Channel.Opcode;
using ChannelServer.Map;
using ChannelServer.Packet;
using ChannelServer.Services;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using Common.Handle;
using Common.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            CCharacter mapleCharacter = CMapleCharacter.LoadData(cid, client);
            client.SendDatat(PlayerPakcet.GetCharInfo(ChannelServices.ChannelId, mapleCharacter, client));
            Console.WriteLine("角色信息:" + mapleCharacter.Name);
            //进行添加到地图列表中...
            MapleMapFactory.MapFactory.GetMap(client.CharacterInfo.character.MapId).AddPlayer(client.CharacterInfo);            

            //CMapleMap.AddPlayer(mapleCharacter.MapId, client.CharacterInfo);
        }
    }

    //TODO:更换频道(100%)
    [PacketHead(RecvOpcode.频道更换, typeof(RecvOpcode))]
    public class ChangeChannel : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            byte ChannelId = mapleBuffer.read<byte>();

            ChannelInfo state = new ChannelInfo();
            Task.Run(async () =>
            {
                state = (await ChannelServices.sChannelService.GetChannelInfo(ChannelId));                
            }).Wait();

            client.SendDatat(PlayerPakcet.GetChannelChange(state.Address, state.port));
            //把该玩家从地图中删除对象.
            MapleMapFactory.MapFactory.GetMap(client.CharacterInfo.character.MapId).RemovePlayer(client.CharacterInfo);
        }
    }



}
