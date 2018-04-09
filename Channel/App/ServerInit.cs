using ChannelServer.Config;
using Common.Attribute;
using Common.Global;
using Common.Handle;
using Common.ServicesInterface;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc;
using Rabbit.Rpc.ProxyGenerator;
using Rabbit.Transport.DotNetty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChannelServer.App
{
    public class ServerInit:Common.Handler.ServerInterface
    {
        AppConfig config;
        public object GetAppConfig()
        {
            return config;
        }

        public ServerInit()
        {
            config = AppConfig.Load();
            if (config == null)
            {
                AppConfig.Creator();
                config = AppConfig.Load();
            }

#region 注册事件
            Type[] type = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in type)
            {
                Attribute attribute = t.GetCustomAttribute(typeof(PacketHead), true);
                if (attribute != null)
                {
                    HandlerInterface @interface = (HandlerInterface)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    CommonGlobal.mHandler.Add(((PacketHead)attribute).Head, @interface);
                    Console.WriteLine("注册事件:{0}-{1}", System.Enum.GetName(typeof(Channel.Opcode.RecvOpcode), ((PacketHead)attribute).Head), @interface.ToString());
                }
            }
#endregion

            Console.WriteLine("事件注册数量:{0}", CommonGlobal.mHandler.Count);

            var serviceCollection = new ServiceCollection();

            var builder = serviceCollection
                .AddLogging()
                .AddClient()
                .UseSharedFileRouteManager("d:\\routes.txt");

            IServiceProvider serviceProvider = null;
            builder.UseDotNettyTransport();
            serviceProvider = serviceCollection.BuildServiceProvider();

            var serviceProxyGenerater = serviceProvider.GetRequiredService<IServiceProxyGenerater>();
            var serviceProxyFactory = serviceProvider.GetRequiredService<IServiceProxyFactory>();
            var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(ChannelInterface) }).ToArray();

            //创建IUserService的代理。
            ChannelInterface userService = serviceProxyFactory.CreateProxy<ChannelInterface>(services.Single(typeof(ChannelInterface).IsAssignableFrom));
            

            Task.Run(async () =>
            {
                Console.WriteLine("开启!!!");
                //await userService.JoinUser(new UserModel { Age = 100, Name = "你好!" }))
                ChannelModel channel = new ChannelModel(1000);
                ReturnState state = (await userService.RegisterServices(channel));
                Console.WriteLine("开启!!!5555");

                if (state.Error == 0)
                {
                    config.Port = state.Port;
                    config.Index = state.Index;
                }
                else
                {
                    Console.WriteLine("注册失败!!!");
                }


            }).Wait();
        }
    }
}
