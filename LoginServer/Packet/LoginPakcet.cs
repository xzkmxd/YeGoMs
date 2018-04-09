using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using Common.ServicesInterface;
using LoginServer.Opcode;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Packet
{
    public class LoginPakcet
    {

        /// <summary>
        /// 验证版本
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHello(short mapleVersion, byte[] sendIv, byte[] recvIv)
        {
            using (MapleBuffer mapleBuffer = new MapleBuffer())
            {
                mapleBuffer.add((short)0x0E);
                mapleBuffer.add<short>(mapleVersion);
                mapleBuffer.add(recvIv);
                mapleBuffer.add(sendIv);
                mapleBuffer.add(4);

                return mapleBuffer.ToArray();
            }
        }

        /// <summary>
        /// 登陆状态
        /// <para>0x03:你已被封号</para>
        /// <para>0x04:密码错误</para>
        /// <para>0x05:未登陆的帐号</para>
        /// <para>0x06:系统错误,无法连接</para>
        /// <para>0x07:现在连接的帐号或正在检查服务器.</para>
        /// <para>0x08:系统错误,无法连接</para>
        /// <para>0x09:系统错误,无法连接</para>
        /// <para>0x0A:服务器忙,无法处理你的请求</para>
        /// <para>0x0B:只有20岁以上的用户可以连接</para>
        /// <para>0x0D:现在的IP不能做MASTER连接</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        [PacketHead(SendOpcode.登录状态, typeof(SendOpcode))]
        public static MaplePakcet getLoginFailed(byte type)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(type);
                buffer.add<byte>(0);
                buffer.add(new byte[1000]);
                return new MaplePakcet(buffer.ToArray());
            }
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        [PacketHead(SendOpcode.心跳包, typeof(SendOpcode))]
        public static MaplePakcet Ping()
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                return new MaplePakcet(buffer.ToArray());
            }
        }



        /// <summary>
        /// 显示注册
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        [PacketHead(SendOpcode.显示注册, typeof(SendOpcode))]
        public static MaplePakcet ShowRegister(bool isShow)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<bool>(isShow);
                return new MaplePakcet(buffer.ToArray());
            }
        }

        /// <summary>
        /// 注册反馈
        /// </summary>
        /// <returns></returns>
        [PacketHead(SendOpcode.注册帐号, typeof(SendOpcode))]
        public static MaplePakcet RegisterAccount(bool Success)
        {

            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add(Success);

                return new MaplePakcet(buffer.ToArray());
            }
        }

        /// <summary>
        /// 帐号检测
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Success"></param>
        /// <returns></returns>
        [PacketHead(SendOpcode.检测帐号, typeof(SendOpcode))]
        public static MaplePakcet CheckAccount(string Name, bool Success)
        {
            using (MapleBuffer buff = new MapleBuffer())
            {
                buff.add<string>(Name);
                buff.add<bool>(Success);
                return new MaplePakcet(buff.ToArray());
            }
        }


        /// <summary>
        /// 帐号登陆消息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [PacketHead(SendOpcode.登录状态, typeof(SendOpcode))]
        public static MaplePakcet getAuthSuccessRequest(CUser user)
        {
            //01 00 01 00 00 00 00 00 06 00 78 7A 6B 6D 78 64 01 00 00 00 01
            //01 04 01 00 00 00 00 00 06 00 78 7A 6B 6D 78 64 01 00 00 00 01
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(0);
                buffer.add<int>(user.Id);
                buffer.add<byte>(0);
                buffer.add<byte>((byte)(user.Gm ?? 0));//是否管理员
                buffer.add<string>(user.Name);
                buffer.add<int>(user.Id);
                buffer.add<byte>(0);

                return new MaplePakcet(buffer.ToArray());
            }
        }


        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        [PacketHead(SendOpcode.服务器列表, typeof(SendOpcode))]
        public static MaplePakcet getServerList()
        {
            //05 00 05 00 C3 B0 58 B5 BA 02 08 00 C0 B6 CE CF C5 A3 2D 31 00 00 00 00 00 00 00 08 00 C0 B6 CE CF C5 A3 2D 32 00 00 00 00 00 01 00
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(0);//小区名称(0：蓝蜗牛,1: 蘑菇仔,2: 绿水灵...)
                buffer.add<string>("027");//世界名称
                buffer.add<byte>(2);//频道个数
                for (int i = 0; i < 2; i++)
                {
                    buffer.add<string>("Chal1-" + i);//广告牌
                    buffer.add<int>(0);//在线人数.
                    buffer.add<byte>(0);//世界ID
                    buffer.add<byte>(0);//频道ID
                    buffer.add<byte>(0);
                }

                return new MaplePakcet(buffer.ToArray());
            }
        }

        [PacketHead(SendOpcode.服务器列表, typeof(SendOpcode))]
        public static MaplePakcet getServerList(int id,WroldModel worldModel)
        {
            //05 00 05 00 C3 B0 58 B5 BA 02 08 00 C0 B6 CE CF C5 A3 2D 31 00 00 00 00 00 00 00 08 00 C0 B6 CE CF C5 A3 2D 32 00 00 00 00 00 01 00
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>((byte)id);//小区名称(0：蓝蜗牛,1: 蘑菇仔,2: 绿水灵...)
                buffer.add<string>("027");//世界名称
                buffer.add<byte>((byte)worldModel.m_ChannelList.Count);//频道个数
                for (int i = 0; i < worldModel.m_ChannelList.Count; i++)
                {
                    buffer.add<string>("" + i);//广告牌
                    buffer.add<int>(0);//在线人数.
                    buffer.add<byte>(0);//世界ID
                    buffer.add<byte>(0);//频道ID
                    buffer.add<byte>(0);
                }

                return new MaplePakcet(buffer.ToArray());
            }
        }


        /// <summary>
        /// 获取服务器列表结束
        /// </summary>
        /// <returns></returns>
        [PacketHead(SendOpcode.服务器列表, typeof(SendOpcode))]
        public static MaplePakcet getEndOfServerList()
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(0xFF);//世界ID

                return new MaplePakcet(buffer.ToArray());
            }
        }



        [PacketHead(SendOpcode.服务器状态, typeof(SendOpcode))]
        public static MaplePakcet getServerStatus(byte status)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {

                buffer.add<byte>(status);
                if (status != 0)
                {
                    buffer.add(new byte[6]);
                }
                return new MaplePakcet(buffer.ToArray());
            }
        }

        [PacketHead(SendOpcode.人物列表, typeof(SendOpcode))]
        public static MaplePakcet ShowNumber()
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(1);
                buffer.add<int>(5);
                buffer.add<int>(5 + (3 - 5 % 3));
                return new MaplePakcet(buffer.ToArray());
            }
        }

        [PacketHead(SendOpcode.人物列表, typeof(SendOpcode))]
        public static MaplePakcet ShowPlayList(CMapleClient client, Dictionary<CCharacter, Dictionary<short, CItem>> PlayerList, byte worldid)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(0);
                buffer.add<int>(0);
                buffer.add<byte>((byte)PlayerList.Count);
                foreach (KeyValuePair<CCharacter, Dictionary<short, CItem>> chr in PlayerList)
                {
                    addCharEntry(buffer, chr.Key, client,chr.Value);
                }
                return new MaplePakcet(buffer.ToArray());
            }
        }


        [PacketHead(SendOpcode.检测角色名, typeof(SendOpcode))]
        public static MaplePakcet CharNameResponse(string Name, bool ret)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<string>(Name);
                buffer.add<bool>(ret);
                return new MaplePakcet(buffer.ToArray());
            }
        }

        [PacketHead(SendOpcode.增加人物, typeof(SendOpcode))]
        public static MaplePakcet AddPlayer(CMapleClient client, CCharacter chr)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<byte>(0);
                addCharEntry(buffer, chr, client);
                return new MaplePakcet(buffer.ToArray());
            }
        }


        public static void addCharEntry(MapleBuffer buffer, CCharacter chr, CMapleClient client, Dictionary<short, CItem> dictionary = null)
        {
            addCharStats(buffer, chr, client);

            if (client.CharacterInfo != null)
            {

                foreach (KeyValuePair<short, CItem> Equip in client.CharacterInfo.GetMapleInventory(Common.Client.Inventory.InventoryType.佩戴).getInventory())
                {
                    buffer.add<byte>(byte.Parse(Equip.Key.ToString()));
                    buffer.add<int>(Equip.Value.ItemId);
                }
            }
            else
            {
                foreach (KeyValuePair<short, CItem> Equip in dictionary)
                {
                    buffer.add<byte>(byte.Parse(Equip.Key.ToString()));
                    buffer.add<int>(Equip.Value.ItemId);
                }
            }
            

            buffer.add<byte>(0);

            //Dictionary<byte, int> maskedEquip = new Dictionary<byte, int>();
            //maskedEquip.Add(1, 1002067);
            //foreach (KeyValuePair<byte, int> Equip in maskedEquip)
            //{
            //    buffer.add<byte>(Equip.Key);
            //    buffer.add<int>(Equip.Value);
            //}

            buffer.add<byte>(0);

        }

        /// <summary>
        /// 玩家基本消息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="chr"></param>
        /// <param name="client"></param>
        public static void addCharStats(MapleBuffer buffer, CCharacter chr, CMapleClient client)
        {
            buffer.add<int>(chr.Id);
            buffer.add(chr.Name, 0x13);
            buffer.add<byte>(client.UserInfo.Gender);
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


        [PacketHead(SendOpcode.服务器IP,typeof(SendOpcode))]
        public static MaplePakcet getServerIP(byte[] IPaddr,short port,int chrid)
        {
            using (MapleBuffer buffer = new MapleBuffer())
            {
                buffer.add<short>(0);
                buffer.add(IPaddr);
                buffer.add<short>(port);
                buffer.add<int>(chrid);
                buffer.add<byte>(0);

                return new MaplePakcet(buffer.ToArray());
            }
        }


    }
}
