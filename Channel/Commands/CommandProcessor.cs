using Common.Attribute;
using Common.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ChannelServer.Commands
{
    //TODO:游戏命令工厂(100%)
    public class CommandProcessor
    {
        private static CommandProcessor m_Processor = new CommandProcessor();

        public static CommandProcessor Processor
        {
            get
            {
                if (m_Processor == null)
                {
                    m_Processor = new CommandProcessor();
                }
                return m_Processor;
            }
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        public void Init()
        {
            if (m_Processor == null)
            {
                m_Processor = new CommandProcessor();
            }
        }

        public Dictionary<string, KeyValuePair<CommanAttribute, CommandExecute>> m_Command = new Dictionary<string, KeyValuePair<CommanAttribute, CommandExecute>>();

        public CommandProcessor()
        {

            Type[] type = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in type)
            {
                Attribute attribute = t.GetCustomAttribute(typeof(CommanAttribute), true);
                if (attribute != null)
                {

                    CommandExecute @interface = (CommandExecute)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    m_Command.Add(((CommanAttribute)attribute).Name, new KeyValuePair<CommanAttribute, CommandExecute>(((CommanAttribute)attribute), @interface));
                    Console.WriteLine("命令注册:{0}-{1}", ((CommanAttribute)attribute).Name, @interface.ToString());
                }
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Commans"></param>
        /// <param name="client"></param>
        public void Execute(string Name, string[] Commans, CMapleClient client)
        {
            try
            {
                if (!m_Command.ContainsKey(Name))
                {
                    //找不到该命令.
                    Console.WriteLine(@"该""{0}""命令不存在!", Name);
                }
                else
                {

                    if (Commans.Length < m_Command[Name].Key.Parameters)
                    {
                        Console.WriteLine(@"""{0}""命令使用错误:{1}", m_Command[Name].Key.Name, m_Command[Name].Key.Explain);
                    }
                    else if (m_Command[Name].Value.Execute(client, Commans) >= 1)
                    {
                        Console.WriteLine(@"""{0}"":{0}执行完毕!", Name);
                    }
                    else
                    {
                        Console.WriteLine(@"""{0}"":{0}执行失败!", Name);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"""{0}""命令异常:{1}", Name, e.Message);
            }
        }

    }

    //TODO:游戏命令执行接口(100%)
    public interface CommandExecute
    {
        int Execute(CMapleClient client, String[] paramArrayOfString);
    }

}
