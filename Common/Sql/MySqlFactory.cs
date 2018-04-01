using Chloe.Infrastructure;
using Chloe.MySql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Common.Sql
{

    public enum ConnectType
    {
        SqlServer,
        MySql,
        Oracle,
        SQLite,
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class ConnectionConfigure
    {
        public ConnectType type = ConnectType.MySql;
        public string ServerIp = "127.0.0.1";
        public string UserName = "root";
        public string UserPwd = "root";
        public string DataName = "mydatabase";
        public short port = 3306;
    }

    public class MySqlFactory : IDbConnectionFactory
    {

        private static MySqlFactory Factory;

        private MySqlContext context;
        public static MySqlContext GetFactory
        {
            get
            {
                if(Factory == null)
                {
                    new MySqlFactory(new ConnectionConfigure());
                }
                return Factory.context;
            }
        }

        ConnectionConfigure connection;
        public MySqlFactory(ConnectionConfigure connectionConfigure)
        {
            connection = connectionConfigure;
            Factory = this;
            context = new MySqlContext(this);
        }

        public IDbConnection CreateConnection()
        {
            string constructorString = "server=" + connection.ServerIp + ";user id=" + connection.UserName + ";password=" + connection.UserPwd + ";persistsecurityinfo=True;port=" + connection.port + ";database=" + connection.DataName + ";SslMode=none";
            IDbConnection conn = new MySqlConnection(constructorString);
            /*如果有必要需要包装一下驱动的 MySqlConnection*/
            conn = new Chloe.MySql.ChloeMySqlConnection(conn);
            return conn;
        }
    }
}
