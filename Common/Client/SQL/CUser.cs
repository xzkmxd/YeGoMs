using Chloe.Entity;

namespace Common.Client.SQL
{
    /// <summary>
    /// 帐号数据
    /// </summary>
    [Table("users")]
    public class CUser
    {
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Passw { get; set; }

        public byte Gender { get; set; }

        public LOGINSTATE? Loggedin { get; set; }

        public int? Gm { get; set; }
        public System.DateTime? LastLogin { get; set; }

        public string Macs { get; set; }
        public int? ACash { get; set; }
        public int? Mpoints { get; set; }




    }
}
