using ChannelServer.Config;
using Common.ServicesInterface;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc;
using Rabbit.Rpc.ProxyGenerator;
using Rabbit.Transport.DotNetty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelServer.Services
{
    public class ChannelServices
    {
        Common.ServicesInterface.ChannelInterface sChannelService;
        public static int ChannelId { get; set; }
        ChannelInfo channel = null;
        public ChannelServices(AppConfig appConfig)
        {
            var serviceCollection = new ServiceCollection();

            var builder = serviceCollection
                .AddLogging()
                .AddClient()
                .UseSharedFileRouteManager("d:\\WorldServer.txt");

            IServiceProvider serviceProvider = null;
            builder.UseDotNettyTransport();
            serviceProvider = serviceCollection.BuildServiceProvider();

            var serviceProxyGenerater = serviceProvider.GetRequiredService<IServiceProxyGenerater>();
            var serviceProxyFactory = serviceProvider.GetRequiredService<IServiceProxyFactory>();
            var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(Common.ServicesInterface.ChannelInterface) }).ToArray();

            //创建IUserService的代理。
            sChannelService = serviceProxyFactory.CreateProxy<Common.ServicesInterface.ChannelInterface>(services.Single(typeof(Common.ServicesInterface.ChannelInterface).IsAssignableFrom));


            Task.Run(async () =>
            {
                channel = new ChannelInfo();
                channel.Name = "测试区";
                channel.Players = 500;
                RetType state = (await sChannelService.RegisterChannel(channel));

                if (state.Error == 0)
                {
                    appConfig.Port = (short)state.Port;
                    appConfig.Index = (int)state.UID;
                    ChannelId = state.UID;
                    Console.WriteLine("开启成功!!!");
                }
                else
                {
                    Console.WriteLine("注册失败!!!");
                }

            }).Wait();

        }

        public void QuitServer()
        {
            try
            {
                Task.Run(async () =>
                {
                    await sChannelService.RemoveChannel(ChannelId);
                }).Wait();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("错误:" + e.Message);
            }

        }
    }
}
