using Chloe;
using Chloe.Entity;
using Chloe.MySql;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using Common.Handle;
using Common.ServicesInterface;
using Common.Sql;
using LoginServer.Opcode;
using LoginServer.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Handler
{


    [PacketHead(RecvOpcode.帐号登录, typeof(RecvOpcode))]
    public class CUserLogin : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //封包: 01 05 00 61 64 6D 69 6E 05 00 61 61 61 61 61 00 00 E0 4C 68 02 E1 D9 62 59 24 00 00 00 00 BF F5 00 00 00 00

            //TODO:登陆帐号(5%)
            CUser user = new CUser()
            {
                Name = mapleBuffer.read<string>(),
                Passw = mapleBuffer.read<string>(),
            };


            //MySqlContext context = new MySqlContext(MySqlFactory.GetFactory);//MySqlFactory.GetFactory.Query<CUser>();
            IQuery<CUser> q = MySqlFactory.GetFactory.Query<CUser>();

            CUser UserInfo = (CUser)q.Where(a => a.Name.Equals(user.Name)).FirstOrDefault();

            if (UserInfo == null)
            {
                //client.SendDatat(LoginPakcet.getLoginFailed(5));
                //TODO:自动注册功能(100%)
                client.SendDatat(LoginPakcet.ShowRegister(true));
            }
            else
            {
                if (!user.Passw.Equals(UserInfo.Passw))
                {
                    System.Console.WriteLine("密码错误..");
                    client.SendDatat(LoginPakcet.getLoginFailed(4));
                    return;
                }
                //TODO:登陆请求(50%)
                client.UserInfo = UserInfo;
                client.SendDatat(LoginPakcet.getAuthSuccessRequest(UserInfo));
                //发送全部世界..
                for(int i = 0;i< WroldServices.m_WroldList.Count;i++)
                {
                    client.SendDatat(LoginPakcet.getServerList(i, WroldServices.m_WroldList[i]));
                }
                client.SendDatat(LoginPakcet.getEndOfServerList());
            }

        }
    }


    [PacketHead(RecvOpcode.帐号注册, typeof(RecvOpcode))]
    public class RegisterUserHandler : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //TODO:帐号注册(已完成)
            string UserNmae = mapleBuffer.read<string>();
            string UserPass = mapleBuffer.read<string>();

            CUserInfo info = new Common.Client.SQL.CUserInfo
            {
                Name = mapleBuffer.read<string>(),
                BirthTime = mapleBuffer.read<string>(),

                HomePhone = mapleBuffer.read<string>(),

            };

            string[] Text = new string[4];
            for (int i = 0; i < 4; i++)
            {
                Text[i] = mapleBuffer.read<string>();
            }

            info.Problem = String.Join(",", Text);

            info.Email = mapleBuffer.read<string>();
            info.IDCard = mapleBuffer.read<string>();
            info.PhoneId = mapleBuffer.read<string>();

            //创建帐号
            CUser user = MySqlFactory.GetFactory.Insert<CUser>(new CUser
            {
                Name = UserNmae,
                Passw = UserPass,
                Gender = mapleBuffer.read<byte>(),
            }
            );
            if (user == null)
            {
                client.SendDatat(LoginPakcet.RegisterAccount(true));
                return;
            }
            info.accid = user.Id;
            MySqlFactory.GetFactory.InsertAsync<CUserInfo>(info);
        }
    }


    [PacketHead(RecvOpcode.帐号检查, typeof(RecvOpcode))]
    public class CheckUserHanlder : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //TODO:帐号检查(已完成)
            CUser user = new CUser()
            {
                Name = mapleBuffer.read<string>(),
            };

            if (MySqlFactory.GetFactory.Query<CUser>().Where(a =>
                a.Name.Equals(user.Name)
            ).FirstOrDefault() == null)
            {
                client.SendDatat(LoginPakcet.CheckAccount(user.Name, false));
            }
            else
            {
                client.SendDatat(LoginPakcet.CheckAccount(user.Name, true));
            }
        }
    }


    [PacketHead(RecvOpcode.大区选择, typeof(RecvOpcode))]
    public class SeleServerList : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //TODO:选择服务器(0%)
            short serverId = mapleBuffer.read<byte>();
            client.SendDatat(LoginPakcet.getServerStatus(0));

        }
    }


    [PacketHead(RecvOpcode.请求角色, typeof(RecvOpcode))]
    public class RequestPlayer : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            int serverid = mapleBuffer.read<byte>();
            int channel = mapleBuffer.read<byte>();

            int id = client.UserInfo.Id;
            Dictionary<CCharacter, Dictionary<short, CItem>> Playerlist = CMapleCharacter.ShowAllCharacter(id, serverid);

            client.SendDatat(LoginPakcet.ShowPlayList(client, Playerlist, (byte)serverid));


            //TODO:获取角色列表(0%)
        }
    }



    [PacketHead(RecvOpcode.检测名字, typeof(RecvOpcode))]
    public class CheckName : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            String Name = mapleBuffer.read<string>();
            bool ret = Common.Tools.MapleCharacterUtil.getIdByName(Name);
            client.SendDatat(LoginPakcet.CharNameResponse(Name, !ret));
        }
    }

    [PacketHead(RecvOpcode.创建角色, typeof(RecvOpcode))]
    public class CreatorPlayer : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            //封包: 0B 08 00 78 7A 6B 6D 78 64 61 35 [20 4E 00 00] [4E 75 00 00] [82 DE 0F 00] [A2 2C 10 00] [81 5B 10 00] [F0 DD 13 00] 04 06 0A 05
            CCharacter character = new CCharacter()
            {
                Name = mapleBuffer.read<string>(),
                Face = mapleBuffer.read<int>(),
                Hair = mapleBuffer.read<int>(),
                Userid = client.UserInfo.Id,
                Party = 1,
                Gm = 0,
                Hp = 50,
                Mp = 50,
                MapId = 0,
                Maxhp = 50,
                Maxmp = 50,
                Job = 0,
                Sp = 0,
                World = 0,
                Level = 1,
                Exp = 0,
            };

            int[] Euqip = new int[4];
            for(int i =0;i<Euqip.Length;i++)
            {
                Euqip[i] = mapleBuffer.read<int>();
            }
            Dictionary<byte, int> dictionary = new Dictionary<byte, int>();

            for(int i = 0;i<Euqip.Length;i++)
            {
                switch(i)
                {
                    case 0:
                        dictionary.Add(5, Euqip[i]);
                        break;
                    case 1:
                        dictionary.Add(6, Euqip[i]);
                        break;
                    case 2:
                        dictionary.Add(7, Euqip[i]);
                        break;
                    case 3:
                        dictionary.Add(9, Euqip[i]);
                        break;
                }
            }

            //dictionary.Add(-)

            character.Str = mapleBuffer.read<byte>();
            character.Dex = mapleBuffer.read<byte>();
            character.Int_ = mapleBuffer.read<byte>();
            character.Luk = mapleBuffer.read<byte>();

            if (CMapleCharacter.CreatorPlayer(client.UserInfo.Id,client, character, dictionary))
            {
                client.SendDatat(LoginPakcet.AddPlayer(client, character));
            }
        }
    }

    [PacketHead(RecvOpcode.开始游戏, typeof(RecvOpcode))]
    public class StartGame : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, CMapleClient client)
        {
            int cid = mapleBuffer.read<int>();
            //
            client.SendDatat(LoginPakcet.getServerIP(new byte[] { 127, 0, 0, 1 }, 7575, cid));

        }
    }

}
