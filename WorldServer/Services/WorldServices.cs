using Common.ServicesInterface;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Rabbit.Rpc;
using Rabbit.Rpc.Codec.ProtoBuffer;
using Rabbit.Rpc.ProxyGenerator;
using Rabbit.Transport.DotNetty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldServer.JobEvent;

namespace WorldServer.Services
{
    public class WorldServices
    {
        static Common.ServicesInterface.WorldInterface sWorldService;
        static Common.ServicesInterface.WorldInfo sWroldInfo;
        int WorldId = 0;
        public WorldServices()
        {
            var serviceCollection = new ServiceCollection();

            var builder = serviceCollection
                .AddLogging()
                .AddClient()
                .UseSharedFileRouteManager("d:\\Login.txt");

            IServiceProvider serviceProvider = null;
            builder.UseDotNettyTransport();
            serviceProvider = serviceCollection.BuildServiceProvider();

            var serviceProxyGenerater = serviceProvider.GetRequiredService<IServiceProxyGenerater>();
            var serviceProxyFactory = serviceProvider.GetRequiredService<IServiceProxyFactory>();
            var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(Common.ServicesInterface.WorldInterface) }).ToArray();

            //创建IUserService的代理。
            sWorldService = serviceProxyFactory.CreateProxy<Common.ServicesInterface.WorldInterface>(services.Single(typeof(Common.ServicesInterface.WorldInterface).IsAssignableFrom));


            Task.Run(async () =>
            {

                sWroldInfo = new WorldInfo();
                sWroldInfo = await sWorldService.RegisterWorld(sWroldInfo);
                Console.WriteLine("本服务器编号:" + sWroldInfo.WorldId + " 服务器名称:" + sWroldInfo.Name);
                Console.Title += " " + sWroldInfo.Name;
                ChannelEntity.world = sWorldService;
                ChannelEntity.worldEntity = sWroldInfo;
            }).Wait();

            WorldEvent().ConfigureAwait(true);            
        }

        public void QuitServer()
        {
            try
            {
                Task.Run(async () =>
                {
                    await sWorldService.RemoveWorld(sWroldInfo.WorldId);
                }).Wait();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("错误:" + e.Message);
            }

        }

        public async Task WorldEvent()
        {
            //获取一个定时器管理者
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            IJobDetail job = JobBuilder.Create<CEventJob>()//.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            //每一秒执行一次..
            .WithCronSchedule("0/1 * * * * ?")
            .Build();

            ////增加定时器
            await sched.ScheduleJob(job, trigger);

            //开启定时器
            await sched.Start();
        }
    }
}
