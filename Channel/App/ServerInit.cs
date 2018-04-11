using ChannelServer.Config;
using ChannelServer.Services;
using Common.Attribute;
using Common.constants;
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
        ChannelServer.Services.ChannelServices channelServices;
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

            channelServices = new Services.ChannelServices(config);

            GameConstants._QuitServer += channelServices.QuitServer;
        }
    }
}
