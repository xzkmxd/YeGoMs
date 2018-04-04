using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.SQL
{
    /// <summary>
    /// 角色道具栏信息
    /// </summary>
    [Table("Inventoryslot")]
    public class CInventoryslot
    {
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        public int Chid { get; set; }
        public int Equip { get; set; }
        public int Use { get; set; }
        public int Setup { get; set; }
        public int Etc { get; set; }
        public int Cash { get; set; }
        public int Elab { get; set; }

    }
}
