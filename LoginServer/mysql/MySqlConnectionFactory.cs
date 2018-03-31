using Chloe.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Chloe.MySql;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace LoginServer.mysql
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public MySqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }

        public IDbConnection CreateConnection()
        {
            string constructorString = "server=127.0.0.1;user id=root;password=root;persistsecurityinfo=True;port=3306;database=mydatabase;SslMode=none";
            IDbConnection conn = new MySqlConnection(constructorString);
            /*如果有必要需要包装一下驱动的 MySqlConnection*/
            conn = new Chloe.MySql.ChloeMySqlConnection(conn);
            return conn;

        }
    }
}
