using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.SQL
{

    /// <summary>
    /// 装备SQL基本信息
    /// </summary>
    [Table("inventoryequipment")]
    public class CEquip
    {

        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public int InventoryitemsId { get; set; }
        public int UpgradeSlots { get; set; }
        public int Level { get; set; }
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
        public int Luk { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Watk { get; set; }
        public int Matk { get; set; }
        public int Wdef { get; set; }
        public int Mdef { get; set; }
        public int Acc { get; set; }
        public int Avoid { get; set; }
        public int Hands { get; set; }
        public int Speed { get; set; }
        public int Jump { get; set; }
        public string Owner { get; set; }
    }
}
