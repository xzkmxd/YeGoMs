using Common.ServicesInterface;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Rabbit.Rpc;
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
        WroldInterface userService;
        static WroldModel wrold;
        int WorldId = 0;
        public WorldServices()
        {
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
            var services = serviceProxyGenerater.GenerateProxys(new[] { typeof(Common.ServicesInterface.WroldInterface) }).ToArray();

            //创建IUserService的代理。
            userService = serviceProxyFactory.CreateProxy<Common.ServicesInterface.WroldInterface>(services.Single(typeof(Common.ServicesInterface.WroldInterface).IsAssignableFrom));


            Task.Run(async () =>
            {
                wrold = new WroldModel();
                wrold = await userService.RegisterServices(wrold);
                WorldId = await userService.GetSize();
                Console.WriteLine("本服务器编号:" + wrold.ID + " 服务器名称:" + WroldModel.GetName(wrold.ID));
                Console.Title += " " + WroldModel.GetName(WorldId);                
            }).Wait();

            WorldEvent().ConfigureAwait(true);            
        }

        public void QuitServer()
        {
            try
            {
                Task.Run(async () =>
                {
                    await userService.RemoveWorld(wrold.ID);
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
