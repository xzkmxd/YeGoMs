using Common.constants;
using Common.ServicesInterface;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Runtime.Server;
using Rabbit.Transport.DotNetty;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.App
{
    class ServerInit:Common.Handler.ServerInterface
    {
        static WorldServer.Services.WorldServices services;
        static WorldServer.Services.ChannelServices channelServices;

        public ServerInit()
        {
            services = new WorldServer.Services.WorldServices();
            //监控退出函数(因该是世界服务器.所以只是向登陆服务器进行注销对象.且没有进行全部玩家保存数据.)
            GameConstants._QuitServer += services.QuitServer;
            channelServices = new Services.ChannelServices();
        }

        public object GetAppConfig()
        {
            throw new NotImplementedException();
        }
    }
}
