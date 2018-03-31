using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.mysql
{
    [TableAttribute("Users")]
    public class User
    {
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
