using Common.Client.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Client.Inventory
{
    public enum InventoryType
    {
        佩戴,
        未知,
        装备,
        消耗,
        设置,
        其他,
        现金,
        仓库
    }

    /// <summary>
    /// 道具栏数据
    /// </summary>
    public class CMapleInventory
    {
        private Dictionary<short, CItem> Inventory;
        private byte slotLimit  = 32;
        /// <summary>
        /// 道具栏
        /// </summary>
        private InventoryType type;
        private byte maxSlot = 96;

        public CMapleInventory(InventoryType type)
        {
            Inventory = new Dictionary<short, CItem>();
            this.type = type;
        }

        public CItem GetItem(short slot)
        {
            return Inventory[slot];
        }

        public bool isFull()
        {
            return this.Inventory.Count >= this.slotLimit;
        }

        public void removeSlot(short slot)
        {
            this.Inventory.Remove(slot);
        }

        public short getNextFreeSlot()
        {
            if (isFull())
            {
                return -1;
            }
            for (short i = 1; i <= this.slotLimit; i = (short)(i + 1))
            {
                if (!this.Inventory.ContainsKey(i))
                {
                    return i;
                }
            }
            return -1;
        }


        public short AddItem(CItem item)
        {
            short slotId = getNextFreeSlot();
            if (slotId < 0)
            {
                return -1;
            }

            item.Position = (byte)slotId;
            this.Inventory.Add(slotId, item);
            Sql.MySqlFactory.GetFactory.Insert<CItem>(item);
            return slotId;
        }

        public void addFromDB(CItem item)
        {
            //if ((item.getPosition() < 0) && (!this.type.equals(MapleInventoryType.EQUIPPED)))
            //{
            //    return;
            //}
            //if ((item.getPosition() > 0) && (this.type.equals(MapleInventoryType.EQUIPPED)))
            //{
            //    return;
            //}
            this.Inventory.Add((short)item.Position, item);
            Sql.MySqlFactory.GetFactory.Insert<CItem>(item);
        }

        public void SevsDB()
        {
            foreach(KeyValuePair<short,CItem> item in Inventory)
            {
                int xxx = Sql.MySqlFactory.GetFactory.Query<CItem>().Where(a => (a.Cid == item.Value.Cid && a.Id == item.Value.Id)).Count();
                if (xxx > 0)
                {

                }
                else
                {
                    Sql.MySqlFactory.GetFactory.Insert<CItem>(item.Value);
                }
            }
        }

        /// <summary>
        /// 获取栏数量
        /// </summary>
        /// <returns></returns>
        public byte getSlotLimit()
        {
            return this.slotLimit;
        }

        /// <summary>
        /// 设置栏的限制
        /// </summary>
        /// <param name="slot"></param>
        public void setSlotLimit(byte slot)
        {
            if(slot > maxSlot)
            {
                slot = maxSlot;
            }
            this.slotLimit = slot;
        }

        public void addSlot(byte slot)
        {
            this.slotLimit = (byte)(this.slotLimit + slot);
            if (this.slotLimit > this.maxSlot)
            {
                this.slotLimit = this.maxSlot;
            }
        }


        /// <summary>
        /// 根据道具ID进行查找
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public CItem FindById(int ItemId)
        {
            foreach(KeyValuePair<short,CItem> item in Inventory)
            {
                if(item.Value.ItemId == ItemId)
                {
                    return item.Value;
                }
            }

            return null;
        }


        public Dictionary<short, CItem> getInventory()
        {
            return Inventory;
        }



    }
}
