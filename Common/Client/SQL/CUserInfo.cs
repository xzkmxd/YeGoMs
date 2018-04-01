using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.SQL
{   
    [Table("userinfo")]
    public class CUserInfo
    {
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int accid { get; set; }

        public string BirthTime { get; set; }

        public string HomePhone { get; set; }

        public string Problem { get; set; }

        public string Email { get; set; }

        public string IDCard { get; set; }

        public string PhoneId { get; set; }

    }
}
