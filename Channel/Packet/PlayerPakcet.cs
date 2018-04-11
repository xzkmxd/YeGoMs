using Channel.Opcode;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChannelServer.Packet
{
    public class PlayerPakcet
    {

        //TODO:游戏公告(10%)
        [PacketHead(SendOpcode.服务器公告, typeof(SendOpcode))]
        public static MaplePakcet ServerMessage(int type, string Message, int channel = 0, bool megaEar = false)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>((byte)type);
                buffer.add<string>(Message);
                switch (type)
                {
                    case 3:
                        buffer.add<byte>((byte)channel);
                        buffer.add<byte>(megaEar ? (byte)1 : (byte)0);
                        break;
                }
                return new MaplePakcet(buffer.ToArray());
            }
        }

        //TODO:进入游戏(20%)
        [PacketHead(SendOpcode.进入游戏, typeof(SendOpcode))]
        public static MaplePakcet GetCharInfo(int Channelid, CCharacter chr, CMapleClient client)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<int>(Channelid);//频道
                buffer.add<byte>(0);
                buffer.add<byte>(1);
                buffer.add<int>(new System.Random().Next());
                buffer.add<int>(new System.Random().Next());
                buffer.add<int>(new System.Random().Next());
                buffer.add<int>(0);

                AddCharacterInfo(buffer, chr, client);
                return new MaplePakcet(buffer.ToArray());
            }
        }

        public static void AddCharacterInfo(MapleBuffer buffer, CCharacter chr, CMapleClient client)
        {
            buffer.add<short>(-1);
            AddCharStats(buffer, chr, client);
            buffer.add<byte>(20);
            buffer.add<int>(0);//金币

            //装备与道具
            AddInventoryInfo(buffer, client.CharacterInfo);

            //技能
            buffer.add<short>(0);

            //任务
            buffer.add<short>(0);
            buffer.add<short>(0);
            //戒指
            buffer.add<short>(0);
            for (int i = 0; i < 5; i++)
            {
                buffer.add<int>(0);
            }


        }

        public static void AddCharStats(MapleBuffer buffer, CCharacter chr, CMapleClient client)
        {
            buffer.add<int>(chr.Id);
            buffer.add(chr.Name, 0x13);
            buffer.add<byte>(0);//性别
            buffer.add<byte>(byte.Parse(chr.Skin.ToString()));
            buffer.add<int>(chr.Face);
            buffer.add<int>(chr.Hair);
            buffer.add<long>(0);
            buffer.add<byte>((byte)chr.Level);
            buffer.add<short>(chr.Job);
            buffer.add<short>(chr.Str);
            buffer.add<short>(chr.Dex);
            buffer.add<short>(chr.Int_);
            buffer.add<short>(chr.Luk);
            buffer.add<short>(chr.Hp);
            buffer.add<short>(chr.Mp);
            buffer.add<short>(chr.Maxhp);
            buffer.add<short>(chr.Maxmp);
            buffer.add<short>(chr.Ap);
            buffer.add<short>(chr.Sp);
            buffer.add<int>(chr.Exp);
            buffer.add<short>(chr.Fame);
            buffer.add<int>(chr.MapId);
            buffer.add<byte>(chr.Spawnpoint);
            buffer.addTime(150842304000000000L);
            buffer.add<long>(0);
        }


        public static void AddInventoryInfo(MapleBuffer buffer, CMapleCharacter chr)
        {
            List<CItem> equipped = new List<CItem>();
            List<CItem> equippedCash = new List<CItem>();
            //判断是否点装..
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.佩戴).getInventory())
            {
                if (item.Key < 0 && item.Key > -100)
                {
                    equipped.Add(item.Value);
                }
                else if (item.Key <= -100 && item.Key > -1000)
                {
                    equippedCash.Add(item.Value);
                }
            }

            //装备
            foreach (CItem item in equipped)
            {
                AddItemInfo(buffer, item);
            }
            buffer.add<byte>(0);//1
            //现金装备
            foreach (CItem item in equippedCash)
            {
                AddItemInfo(buffer, item);
            }

            buffer.add<byte>(0);//2
            buffer.add<byte>((byte)chr.GetMapleInventory(Common.Client.Inventory.InventoryType.装备).getInventory().Count);//2
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.装备).getInventory())
            {
                AddItemInfo(buffer, item.Value);
            }
            buffer.add<byte>(0);//3
            buffer.add<byte>((byte)chr.GetMapleInventory(Common.Client.Inventory.InventoryType.消耗).getInventory().Count);//2
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.消耗).getInventory())
            {
                AddItemInfo(buffer, item.Value);
            }

            buffer.add<byte>(0);//4
            buffer.add<byte>((byte)chr.GetMapleInventory(Common.Client.Inventory.InventoryType.设置).getInventory().Count);//2
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.设置).getInventory())
            {
                AddItemInfo(buffer, item.Value);
            }

            buffer.add<byte>(0);//5
            buffer.add<byte>((byte)chr.GetMapleInventory(Common.Client.Inventory.InventoryType.其他).getInventory().Count);//2
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.其他).getInventory())
            {
                AddItemInfo(buffer, item.Value);
            }

            buffer.add<byte>(0);//6
            buffer.add<byte>((byte)chr.GetMapleInventory(Common.Client.Inventory.InventoryType.现金).getInventory().Count);//2
            foreach (KeyValuePair<short, CItem> item in chr.GetMapleInventory(Common.Client.Inventory.InventoryType.现金).getInventory())
            {
                AddItemInfo(buffer, item.Value);
            }

            buffer.add<byte>(0);//7

        }

        //TODO:道具信息(0%)
        public static void AddItemInfo(MapleBuffer buffer, CItem item)
        {
            //buffer.add<int>
        }

        //TODO:聊天信息(100%)
        [PacketHead(SendOpcode.聊天信息, typeof(SendOpcode))]
        public static MaplePakcet GetChatText(int cid, string Text, bool whiteBG = false)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<int>(cid);
                buffer.add<byte>(whiteBG ? (byte)1 : (byte)0);//是否显示说话对话框
                buffer.add<string>(Text);
                return new MaplePakcet(buffer.ToArray());
            }
        }

    }
}
