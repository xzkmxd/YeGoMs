using Chloe;
using Chloe.Entity;
using Chloe.MySql;
using Common.Attribute;
using Common.Buffer;
using Common.Client;
using Common.Client.SQL;
using Common.Handle;
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
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
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

            CUser xx = (CUser)q.Where(a => a.Name.Equals(user.Name)).FirstOrDefault();

            if (xx == null)
            {
                //client.SendDatat(LoginPakcet.getLoginFailed(5));
                //TODO:自动注册功能(100%)
                client.SendDatat(LoginPakcet.ShowRegister(true));
            }
            else
            {
                if (!user.Passw.Equals(xx.Passw))
                {
                    System.Console.WriteLine("密码错误..");
                    client.SendDatat(LoginPakcet.getLoginFailed(4));
                    return;
                }
                //TODO:登陆请求(50%)
                client.SendDatat(LoginPakcet.getAuthSuccessRequest(xx));
                client.SendDatat(LoginPakcet.getServerList());
                client.SendDatat(LoginPakcet.getEndOfServerList());
            }

        }
    }


    [PacketHead(RecvOpcode.帐号注册, typeof(RecvOpcode))]
    public class RegisterUserHandler : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
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
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
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
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
        {
            //TODO:选择服务器(0%)
            short serverId = mapleBuffer.read<byte>();
            client.SendDatat(LoginPakcet.getServerStatus(0));

        }
    }


    [PacketHead(RecvOpcode.请求角色, typeof(RecvOpcode))]
    public class RequestPlayer : HandlerInterface
    {
        public override void Handle(MapleBuffer mapleBuffer, MapleClient client)
        {
            int serverid = mapleBuffer.read<byte>();
            int channel = mapleBuffer.read<byte>();
            
            //TODO:获取角色列表(0%)
        }
    }



}
