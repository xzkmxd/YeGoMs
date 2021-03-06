﻿using System;
using DotNetty.Buffers;
using Common.Buffer;
using Common.Handle;
using System.Reflection;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using System.Security.Cryptography.X509Certificates;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Codecs;
using System.Collections.Generic;
using DotNetty.Common.Utilities;
using ServerApp.Handler;
using System.Threading;
using System.Xml;
using Common.Handler;
using System.Runtime.InteropServices;
using Common.constants;

namespace ServerApp
{
    public delegate bool ConsoleCtrlDelegate(int dwCtrlType);        

    class Program
    {
        

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);
        private const int CtrlCEvent = 0;//CTRL_C_EVENT = 0;//一个Ctrl+C的信号被接收,该信号或来自键盘,或来自GenerateConsoleCtrlEvent    函数   
        private const int CtrlBreakEvent = 1;//CTRL_BREAK_EVENT = 1;//一个Ctrl+Break信号被接收,该信号或来自键盘,或来自GenerateConsoleCtrlEvent    函数  
        private const int CtrlCloseEvent = 2;//CTRL_CLOSE_EVENT = 2;//当用户系统关闭Console时,系统会发送此信号到此   
        private const int CtrlLogoffEvent = 5;//CTRL_LOGOFF_EVENT = 5;//当用户退出系统时系统会发送这个信号给所有的Console程序。该信号不能显示是哪个用户退出。   
        private const int CtrlShutdownEvent = 6;//CTRL_SHUTDOWN_EVENT = 6;//当系统将要关闭时会发送此信号到所有Console程序   

        static bool Run = true;
        static Assembly assembly;
        static object AssemblyObj;
        static string AppPath = System.IO.Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            Console.Clear();

            #region 设置控制台事件
            SetConsoleCtrlHandler(new ConsoleCtrlDelegate((a) =>
            {
                switch (a)
                {
                    case CtrlCloseEvent:
                        {
                            Console.WriteLine("正在保存数据中....");
                            if(GameConstants._QuitServer!=null)
                            {
                                GameConstants._QuitServer();//.Invoke();
                            }
                            break;
                        }
                }
                return true;
            }
            ), true);
            #endregion
#if 测试版本
            #region 测试反射例子
            string path3 = System.IO.Directory.GetCurrentDirectory();
            
            assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + path3 + "\\LoginServer.dll"));

            while (true)
            {

                MapleBuffer mapleBuffer = new MapleBuffer();
                mapleBuffer.add<int>(50);
                mapleBuffer.add<int>(50);


                HandlerInterface game = (HandlerInterface)assembly.CreateInstance("LoginServer.Handler.TestGame");
                game.Handle(mapleBuffer);
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "0":
                        {
                            assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + path3 + "\\LoginServer.dll"));
                            break;
                        }
                }
            }
            #endregion
#endif
#if !正式版测试

            Console.Title = "启动:" + args[0];
            #region 服务端命令
            switch (args[0])
            {
                case "Common":
                case "LoginServer":
                    {
                        LoadConfig(args[0]).ConfigureAwait(true);                       
                        break;
                    }
                case "ChannelServer":
                    LoadConfig(args[0], true).Wait();
                    break;
                case "WorldServer":                    
                    LoadConfig(args[0], true,true).ConfigureAwait(true);
                    break;
                case "GameServer":
                case "DBServer":
                    {
                        LoadConfig(args[0], true).ConfigureAwait(true);
                        break;
                    }
                default:
                    Console.WriteLine("启动错误!");
                    break;
            }

            while (Run)
            {
                string linet = System.Console.ReadLine();
                switch (linet)
                {
                    case "Exit":
                        Environment.Exit(0);
                        break;
                    case "Test":
                        assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + AppPath + "\\" + args[0] + ".dll"));
                        assembly.CreateInstance(args[0] + ".App.ServerInit");
                        break;
                }
            }

            #endregion
#endif

        }

        #region 加载配置且开启服务
        static async Task LoadConfig(string args, bool isChannel = false,bool isWorld = false)
        {

            await Task.Run(() =>
            {
                assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + AppPath + "\\" + args + ".dll"));
                AssemblyObj = assembly.CreateInstance(args + ".App.ServerInit");

            });
            if(!isWorld)
            {
                if (isChannel)
                {
                    //获取World的端口
                    short login = ((AppConfigInterface)((Common.Handler.ServerInterface)AssemblyObj).GetAppConfig()).GetPort();
                    System.Console.WriteLine("频道端口:{0}", login);
                    RunServerAsync(login, args).Wait();//.Start();
                }
                else
                {
                    short login = ((AppConfigInterface)((Common.Handler.ServerInterface)AssemblyObj).GetAppConfig()).GetPort();
                    System.Console.WriteLine("登陆端口:{0}", login);
                    RunServerAsync(login, args).Wait();//.Start();
                }
            }
        }
        #endregion


        #region 运行Dotnetty
        static async Task RunServerAsync(short Port, string ServerName)
        {
            //ExampleHelper.SetConsoleLogger();
            // 主工作线程组,设置为1个线程
            var bossGroup = new MultithreadEventLoopGroup(1);
            // 工作线程组,默认为内核数*2的线程数
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {

                //声明一个服务端Bootstrap,每个Netty服务端程序,都由ServerBootstrap控制,
                //通过链式的方式组装需要的参数

                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup) // 设置主和工作线程组
                    .Channel<TcpServerSocketChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.SoBacklog, 100) // 设置网络IO参数等,这里可以设置很多参数,当然你对网络调优和参数设置非常了解的话,你可以设置,或者就用默认参数吧
                                                          //.Handler(new LoggingHandler("SRV-LSTN")) //在主线程组上设置一个打印日志的处理器
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    { //工作线程连接器 是设置了一个管道,服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                      //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        //出栈消息,通过这个handler 在消息顶部加上消息的长度
                        pipeline.AddLast("framing-enc", new MapleEncoder());// new MapleEncoder555());
                        //入栈消息通过该Handler,解析消息的包长信息,并将正确的消息体发送给下一个处理Handler,该类比较常用,后面单独说明
                        pipeline.AddLast("framing-dec", new MapleDecoder());
                        //业务handler ,这里是实际处理Echo业务的Handler
                        pipeline.AddLast(ServerName, new MapleServerHandler());
                    }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务,同样的Serverbootstrap可以bind到多个端口
                IChannel boundChannel = await bootstrap.BindAsync(Port);
                while (Run)
                {
                    string linet = System.Console.ReadLine();
                    switch (linet)
                    {
                        case "Exit":
                            Environment.Exit(0);
                            break;
                    }
                }

                //关闭服务
                //await boundChannel.CloseAsync();
            }
            finally
            {
                //释放工作组线程
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }

        }
        #endregion



    }
}
