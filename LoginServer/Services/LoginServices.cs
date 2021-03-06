﻿using Common.ServicesInterface;
using LoginServer.Services.Entity;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Codec.ProtoBuffer;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Runtime.Server;
using Rabbit.Transport.DotNetty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Services
{
    /// <summary>
    /// 注册世界监听服务
    /// </summary>
    public class LoginServices
    {
        public LoginServices()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var serviceCollection = new ServiceCollection();
            var builder = serviceCollection
                .AddLogging()
                .AddRpcCore()
                .AddServiceRuntime()
                .UseSharedFileRouteManager("d:\\Login.txt")
                .UseDotNettyTransport()
                ;


            //serviceCollection.AddTransient<ChannelInterface, ChannelServices>();

            //serviceCollection.AddTransient<Common.ServicesInterface.ChannelInterface, Common.ServicesInterface.ChannelServices>();
            serviceCollection.AddTransient<WorldInterface,WorldEntity>();

            IServiceProvider serviceProvider = null;
            serviceProvider = serviceCollection.BuildServiceProvider();

            //serviceProvider.GetRequiredService<ILoggerFactory>()
            //    .AddConsole((c, l) => (int)l >= 3);

            {
                var serviceEntryManager = serviceProvider.GetRequiredService<IServiceEntryManager>();
                var addressDescriptors = serviceEntryManager.GetEntries().Select(i => new ServiceRoute
                {
                    Address = new[] { new IpAddressModel { Ip = "127.0.0.1", Port = 9981 } },
                    ServiceDescriptor = i.Descriptor
                });

                var serviceRouteManager = serviceProvider.GetRequiredService<IServiceRouteManager>();
                serviceRouteManager.SetRoutesAsync(addressDescriptors).Wait();
            }

            var serviceHost = serviceProvider.GetRequiredService<IServiceHost>();

            Task.Factory.StartNew(async () =>
            {
                //启动主机
                await serviceHost.StartAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9981));
                Console.WriteLine($"监听世界开启,{DateTime.Now}。");
            }).Wait();
        }

    }
}
