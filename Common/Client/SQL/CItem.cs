using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.SQL
{
    /// <summary>
    /// 道具数据
    /// </summary>
    [Table("inventoryitems")]
    public class CItem
    {
        public CItem()
        {
            Owner = "";
        }

        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        public int Type { get; set; }

        public int Cid { get; set; }
        public int UserId { get; set; }

        public int ItemId { get; set; }

        public int InventoryType { get; set; }
        public int Position { get; set; }
        public int Quantity { get; set; }
        public string Owner { get; set; }
        public int Uniqueid { get; set; }
        public int Flag { get; set; }
        public int Expiredate { get; set; }
        public int Sender { get; set; }

    }
}
