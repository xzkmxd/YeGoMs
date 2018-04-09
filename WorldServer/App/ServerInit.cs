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

        public ServerInit()
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var serviceCollection = new ServiceCollection();
            var builder = serviceCollection
                .AddLogging()
                .AddRpcCore()
                .AddClientRuntime()
                .AddServiceRuntime()
                .UseSharedFileRouteManager("d:\\routes.txt");

            builder.UseDotNettyTransport();

            serviceCollection.AddTransient<ChannelInterface, ChannelServices>();
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
                Console.WriteLine($"服务端启动成功，{DateTime.Now}。");
            }).Wait();
            Console.ReadLine();

        }

        public object GetAppConfig()
        {
            throw new NotImplementedException();
        }
    }
}
