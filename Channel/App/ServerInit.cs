using ChannelServer.Config;
using Common.Attribute;
using Common.Global;
using Common.Handle;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Channel.App
{
    public class ServerInit : Common.Handler.ServerInterface
    {
        AppConfig config;
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
                    Console.WriteLine("注册事件:{0}-{1}", System.Enum.GetName(typeof(Opcode.RecvOpcode), ((PacketHead)attribute).Head), @interface.ToString());
                }
            }
            #endregion

            Console.WriteLine("事件注册数量:{0}", CommonGlobal.mHandler.Count);



        }
    }
}
