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

        //TODO:玩家信息(100%)
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

        //TODO:角色基础信息(90%)
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

        //TODO:道具信息(100%)
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

        //TODO:其他类型道具(100%)
        public static void AddOtherItemInfo(MapleBuffer buffer, CItem item)
        {
            AddBaseItemHeader(buffer, item);
            buffer.add<short>((short)item.Quantity);
            buffer.add<string>(item.Owner);
        }

        //TODO:道具基础头部(100%)
        public static void AddBaseItemHeader(MapleBuffer buffer, CItem item)
        {
            buffer.add<int>(item.ItemId);
            buffer.add<byte>(item.Uniqueid > 0 ? (byte)1 : (byte)0);
            if (item.Uniqueid > 0)
            {
                buffer.add<long>(item.Uniqueid);
            }
            buffer.addTime(Common.constants.GameConstants.getTime(item.Expiredate));            //到期时间
        }


        //TODO:道具信息(30%)
        public static void AddItemInfo(MapleBuffer buffer, CItem item)
        {
            //buffer.add<int>
            short Posin = (short)item.Position;
            if(Posin <= -1)
            {
                Posin *= -1;
                if(Posin > 100 && Posin <1000)
                {
                    Posin -= 100;
                }
            }
            buffer.add<byte>((byte)Posin);

            if (false)//宠物道具信息
            {

            }
            else
            {
                if (item.Type == 0 || item.Type == 1)//0:佩戴,1装备
                {
                    //装备道具数据
                    CEquip equip = Common.Sql.MySqlFactory.GetFactory.Query<CEquip>().Where(a => a.InventoryitemsId == item.Id).FirstOrDefault();
                    if (equip == null)
                    {
                        AddBaseItemHeader(buffer, item);
                        buffer.add<byte>(0);
                        buffer.add<byte>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<short>(0);
                        buffer.add<string>("");
                    }
                    else
                    {
                        AddBaseItemHeader(buffer, item);
                        AddEquipItemInfo(buffer, equip);
                    }

                }
                else
                {
                    //其他道具数据
                    AddOtherItemInfo(buffer, item);
                }
            }

        }

        //TODO:装备信息(50%)
        private static void AddEquipItemInfo(MapleBuffer buffer, CEquip equip)
        {
            buffer.add<byte>((byte)equip.UpgradeSlots);
            buffer.add<byte>((byte)equip.Level);
            buffer.add<short>((short)equip.Str);
            buffer.add<short>((short)equip.Dex);
            buffer.add<short>((short)equip.Int);
            buffer.add<short>((short)equip.Luk);
            buffer.add<short>((short)equip.Hp);
            buffer.add<short>((short)equip.Mp);
            buffer.add<short>((short)equip.Watk);
            buffer.add<short>((short)equip.Matk);
            buffer.add<short>((short)equip.Wdef);
            buffer.add<short>((short)equip.Mdef);
            buffer.add<short>((short)equip.Acc);
            buffer.add<short>((short)equip.Avoid);
            buffer.add<short>((short)equip.Hands);
            buffer.add<short>((short)equip.Speed);
            buffer.add<short>((short)equip.Jump);
            buffer.add<string>(equip.Owner);
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

        //TODO:暂时未增加获取IP和端口功能(100%)..
        [PacketHead(SendOpcode.更换频道, typeof(SendOpcode))]
        public static MaplePakcet GetChannelChange(string Address, short port)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(1);
                string[] adds = Address.Split(".");
                foreach (string add in adds)
                {
                    buffer.add(byte.Parse(add.ToString()));//IP地址
                }
                buffer.add<short>(port);
                return new MaplePakcet(buffer.ToArray());
            }
        }


        [PacketHead(SendOpcode.召唤玩家,typeof(SendOpcode),5)]
        public static MaplePakcet SpawnPlayer(CMapleCharacter chr)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<int>(chr.character.Id+1);
                buffer.add<string>(chr.character.Name);
                buffer.add<int>(0);//Buffmask
                addCharLook(buffer, chr);
                buffer.add<int>(0);
                buffer.add<int>(0);//getItemEffect
                buffer.add<int>(0);
                buffer.add<short>((short)chr.GetPoint().X);
                buffer.add<short>((short)chr.GetPoint().Y);
                buffer.add<byte>(chr.GetStance());
                buffer.add<short>(0);
                buffer.add(new byte[3]);
                return new MaplePakcet(buffer.ToArray());
            }
        }

        public static void addCharLook(MapleBuffer buffer, CMapleCharacter chr)
        {
            buffer.add<byte>(0);//性别
            buffer.add<byte>((byte)chr.character.Skin);
            buffer.add<int>(chr.character.Face);
            buffer.add<byte>(0);
            buffer.add<int>(chr.character.Hair);

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
                buffer.add<byte>((byte)item.Position);
                buffer.add<int>(item.ItemId);
            }
            buffer.add<byte>(0xFF);//1
            //现金装备
            foreach (CItem item in equippedCash)
            {
                buffer.add<byte>((byte)item.Position);
                buffer.add<int>(item.ItemId);
            }
            buffer.add<int>(0);//宠物??
        }


        [PacketHead(SendOpcode.删除玩家,typeof(SendOpcode),100)]
        public static MaplePakcet RemovePlayer(int cid)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<int>(cid);
                return new MaplePakcet(buffer.ToArray());
            }
        }


    }
}
