using System;
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

namespace ServerApp
{

    class Program
    {
        static bool Run = true;
        static Assembly assembly;
        static string AppPath = System.IO.Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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

            switch (args[0])
            {
                case "Common":
                case "LoginServer":
                    {

                        Thread thread = new Thread(() => {
                            RunServerAsync(8484).Wait();//.Start();
                        });
                        thread.Start();
                        //开始进行Dot
                        Thread Iothread = new Thread(() => {
                            assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + AppPath + "\\" + args[0] + ".dll"));
                            assembly.CreateInstance(args[0] + ".App.ServerInit");
                        }
                        );
                        Iothread.Start();
                        //热更新.
                        //new Thread(() =>
                        //{
                        //}).Start();
                        
                        while (Run)
                        {
                            string linet = System.Console.ReadLine();
                            switch(linet)
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

                        break;
                    }
                case "GameServer":
                case "DBServer":
                case "ChannelServer":
                    Console.Title = "启动:" + args[0];
                    //进行该DLL的反射机制
                    assembly = Assembly.Load(System.IO.File.ReadAllBytes(@"" + AppPath + "\\" + args[0] + ".dll"));
                    assembly.CreateInstance(args[0] + ".App.ServerInit");


                    while (true)
                    {
                        //进行网络事件注册
                        // LoginServer.App
                        OldMapleBuffer mapleBuffer = new OldMapleBuffer();
                        mapleBuffer.add<short>(5);
                        mapleBuffer.add<int>(50);
                        mapleBuffer.add<int>(50);
                        //Common.Global.CommonGlobal.Run(mapleBuffer.read<short>(), mapleBuffer);
                        Console.ReadLine();

                    }
                    break;
                default:
                    Console.WriteLine("启动错误!");
                    break;
            }
#endif
            //System.Attribute[] xx = AssemblyCultureAttribute.GetCustomAttributes(Assembly.GetCallingAssembly());

            //assembly = Assembly.GetCallingAssembly();
            //assembly.GetCustomAttributes()





            //参数 args[1] 服务端类型,
            /*
            RedisDecoder redisDecoder = new RedisDecoder();
            RedisEncoder redisEncoder = new RedisEncoder();
            */



        }


        static async Task RunServerAsync(short Port)
        {
            //ExampleHelper.SetConsoleLogger();
            // 主工作线程组，设置为1个线程
            var bossGroup = new MultithreadEventLoopGroup(1);
            // 工作线程组，默认为内核数*2的线程数
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {

                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制，
                //通过链式的方式组装需要的参数

                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup) // 设置主和工作线程组
                    .Channel<TcpServerSocketChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.SoBacklog, 100) // 设置网络IO参数等，这里可以设置很多参数，当然你对网络调优和参数设置非常了解的话，你可以设置，或者就用默认参数吧
                    //.Handler(new LoggingHandler("SRV-LSTN")) //在主线程组上设置一个打印日志的处理器
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                      //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                        IChannelPipeline pipeline = channel.Pipeline;
                        //出栈消息，通过这个handler 在消息顶部加上消息的长度
                        pipeline.AddLast("framing-enc", new MapleEncoder());// new MapleEncoder555());
                        //入栈消息通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler，该类比较常用，后面单独说明
                        pipeline.AddLast("framing-dec", new MapleDecoder());
                        //业务handler ，这里是实际处理Echo业务的Handler
                        pipeline.AddLast("Longon", new MapleServerHandler());

                    }));

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                IChannel boundChannel = await bootstrap.BindAsync(Port);
                while (true) ;

                //Console.ReadLine();
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



    }
}
