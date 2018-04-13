using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Packet
{
    public class CommonPacket
    {

        //[PacketHead(SendOpcode.召唤玩家, typeof(SendOpcode), 5)]
        public static MaplePakcet SpawnPlayer(CMapleCharacter chr)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<int>(chr.character.Id);
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
    }
}
