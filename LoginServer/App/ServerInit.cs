using Common.Global;
using Common.Handle;
using LoginServer.Handler;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Attribute;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using Chloe.MySql;
using LoginServer.mysql;
using Chloe;
using Common.Sql;

namespace LoginServer.App
{
    /// <summary>
    /// 该服务器初始化类
    /// </summary>
    public class ServerInit
    {
        public ServerInit()
        {

            #region 自动获取处理

            Type[] type = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in type)
            {
                Attribute attribute = t.GetCustomAttribute(typeof(PacketHead), true);
                if (attribute != null)
                {
                    HandlerInterface @interface = (HandlerInterface)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    CommonGlobal.mHandler.Add(((PacketHead)attribute).Head, @interface);
                    Console.WriteLine(@interface.ToString());
                }

            }
            
            #endregion

            Console.WriteLine("注册事件:{0}", CommonGlobal.mHandler.Count);

            //以下是定时器例子
            Text().Wait();

            //测试mysql数据库

            //IQuery<User> q = MySqlFactory.GetFactory.Query<User>();
            //User xx = q.Where(a => a.Id == 1).FirstOrDefault();

            //for (int i = 0; i < 1000; i++)
            //{
            //    MySqlFactory.GetFactory.Insert<User>(new User
            //    {
            //        Name = i.ToString(),
            //    });
            //}




            //while (true) ;

        }


        #region 定时器测试
        public async Task Text()
        {
            //获取一个定时器管理者
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();

            DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            //.StartAt(runTime)
            .WithCronSchedule("0/5 * * * * ?")
            .Build();

            ////增加定时器
            await sched.ScheduleJob(job, trigger);

            //开启定时器
            await sched.Start();
        }
        #endregion
    }
}
