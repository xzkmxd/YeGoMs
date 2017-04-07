using Common.Handler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ChannelServer.Config
{
    public class AppConfig : AppConfigInterface
    {
        /// <summary>
        /// 黄色公告
        /// </summary>
        public string ServerMessage { get; set; }

        /// <summary>
        /// 经验倍数
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        /// 金钱爆率
        /// </summary>
        public int Meso { get; set; }

        /// <summary>
        /// 掉宝概率
        /// </summary>
        public int Drop { get; set; }

        /// <summary>
        /// Boss概率
        /// </summary>
        public int Bossdrop { get; set; }

        /// <summary>
        /// 宠物经验倍数
        /// </summary>
        public int PetExp { get; set; }

        /// <summary>
        /// 坐骑经验倍数
        /// </summary>
        public int MountExp { get; set; }

        /// <summary>
        /// 事件消息
        /// </summary>
        public string EventMessage { get; set; }

        /// <summary>
        /// 外网地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 事件脚本列表
        /// </summary>
        public string Events { get; set; }

        /// <summary>
        /// 世界服务器地址
        /// </summary>
        public string WorldAddress { get; set; }

        public short GetPort()
        {
            return 0;
        }

        public string GetWorldAddress()
        {
            return WorldAddress;
        }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AppConfig Load(string path = @"..\\ChannelAppConfig.xml")
        {
            AppConfig appConfig = null;
            try
            {
                string Text = System.IO.File.ReadAllText(path);
                XmlDocument xmlq = new XmlDocument();
                xmlq.LoadXml(Text);
                XmlNode rootnode = xmlq.LastChild;
                string nzma = Newtonsoft.Json.JsonConvert.SerializeXmlNode(rootnode, Newtonsoft.Json.Formatting.None, true);
                appConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(nzma);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("加载失败配置文件:" + e);
                return null;
            }

            return appConfig;
        }

        /// <summary>
        /// 创建默认文档
        /// </summary>
        /// <param name="path"></param>
        public static void Creator(string path = @"..\\ChannelAppConfig.xml")
        {
            AppConfig app = new AppConfig()
            {
                Bossdrop = 1,
                Drop = 1,
                EventMessage = "",
                WorldAddress = "127.0.0.1",
                Events = "",
                Exp = 1,
                Host = "127.0.0.1",
                Meso = 1,
                MountExp = 1,
                PetExp = 1,
                ServerMessage = "",
            };
            string xxx = Newtonsoft.Json.JsonConvert.SerializeObject(app);
            Console.WriteLine(xxx);
            XmlDocument xml = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(xxx, "Channel");

            XmlWriterSettings writerSetting = new XmlWriterSettings //声明编写器设置
            {
                Indent = true,//定义xml格式，自动创建新的行
                Encoding = UTF8Encoding.UTF8,//编码格式
                OmitXmlDeclaration = true,//去掉版本号
            };

            XmlWriter writer = XmlWriter.Create(path, writerSetting);
            xml.Save(writer);
        }
    }
}
