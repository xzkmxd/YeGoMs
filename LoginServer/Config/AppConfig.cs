using Common.Handler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LoginServer.Config
{
    public class AppConfig : AppConfigInterface
    {
        /// <summary>
        /// 连接用户个数
        /// </summary>
        public int Userlimit { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public int Flag { get; set; }

        /// <summary>
        /// 每个玩家能创建角色数
        /// </summary>
        public int MaxCharacters { get; set; }

        /// <summary>
        /// 小区个数
        /// </summary>
        public int NumberOfWorlds { get; set; }

        /// <summary>
        /// 世界服务器地址
        /// </summary>
        public string WorldAddress { get; set; }

        /// <summary>
        /// 登陆端口
        /// </summary>
        public short LoginPort { get; set; }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AppConfig Load(string path = @"..\\LoginAppConfig.xml")
        {
            AppConfig appConfig = null;
            try
            {
                string Text = System.IO.File.ReadAllText(path);
                XmlDocument xmlq = new XmlDocument();
                xmlq.LoadXml(Text);
                XmlNode rootnode = xmlq.LastChild;
                string nzma =  Newtonsoft.Json.JsonConvert.SerializeXmlNode(rootnode, Newtonsoft.Json.Formatting.None,true);
                appConfig =  Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(nzma);
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
        public static void Creator(string path = @"..\\LoginAppConfig.xml")
        {
            AppConfig app = new AppConfig();
            app.Flag = 0;
            app.MaxCharacters = 5;
            app.NumberOfWorlds = 1;
            app.ServerName = "C#端";
            app.Userlimit = 100;
            app.WorldAddress = "127.0.0.1";
            app.LoginPort = 8484;
            string xxx = Newtonsoft.Json.JsonConvert.SerializeObject(app);
            Console.WriteLine(xxx);
            XmlDocument xml = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(xxx, "Login");

            XmlWriterSettings writerSetting = new XmlWriterSettings //声明编写器设置
            {
                Indent = true,//定义xml格式,自动创建新的行
                Encoding = UTF8Encoding.UTF8,//编码格式
                OmitXmlDeclaration = true,//去掉版本号
            };

            XmlWriter writer = XmlWriter.Create(path, writerSetting);
            xml.Save(writer);
        }

        public short GetPort()
        {
            return LoginPort;
        }

        public string GetWorldAddress()
        {
            return WorldAddress;
        }

        public short GetId()
        {
            throw new NotImplementedException();
        }
    }
}
