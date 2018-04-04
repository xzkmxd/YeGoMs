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
using Chloe;
using Common.Sql;
using MoonSharp.Interpreter;

namespace LoginServer.App
{
    /// <summary>
    /// 该服务器初始化类
    /// </summary>
    public class ServerInit
    {
        public ServerInit()
        {
            //运行脚本
            //Script.RunFile("Script\\Text.lua");
            //在C#中定义Lua全局脚本
#if Lua
            UserData.RegisterType<TestClass>();
            UserData.RegisterType<GGG>();
           
            Script script = new Script();
            script.Globals["ShowGame"] = (Func<TestClass>)ShowGame;
            //加载脚本.
            try
            {
                DynValue obj = UserData.Create(new TestClass());
                script.Globals.Set("startxx", obj);
                //script.Globals["ShowGame"] = new TestClass();
                //script.Globals.Set("obj", obj);
                DynValue value = script.DoFile("Script\\Text.lua");
                //运行
                script.Call(script.Globals["start"], 10);
            }
            catch (Exception e)
            {
                Console.WriteLine("运行错误:" + e);
            }
#endif


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

        [MoonSharpUserData]
        public class TestClass
        {
            public string Name()
            {
                System.Console.WriteLine("Name");
                return "成功了吗?";
            }

            public GGG GetG(string Name)
            {
                GGG gGG = new GGG();
                gGG.Name = Name;
                return gGG;
            }
        }

        public class GGG
        {
            public string Name
            {
                get;
                set;
            }

            public void Debug()
            {
                Console.WriteLine(Name);
            }


        }


        private TestClass ShowGame()
        {
            TestClass testClass = new TestClass();
            return testClass;
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
