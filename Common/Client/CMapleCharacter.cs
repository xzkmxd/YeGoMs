using Common.Client.Inventory;
using Common.Client.SQL;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 人物角色
/// </summary>
namespace Common.Client
{
    public class CMapleCharacter
    {
        public CCharacter character { get; set; }

        //道具数据
        private CMapleInventory[] inventory;

        public CMapleCharacter()
        {
            inventory = new CMapleInventory[8];
            foreach(InventoryType inv in System.Enum.GetValues(typeof(InventoryType)))
            {
                inventory[(int)inv] = new CMapleInventory(inv);                
            }
        }

        public CMapleCharacter(CCharacter character)
        {
            this.character = character;

            inventory = new CMapleInventory[8];
            foreach (InventoryType inv in System.Enum.GetValues(typeof(InventoryType)))
            {
                inventory[(int)inv] = new CMapleInventory(inv);
            }
        }

        public static CCharacter LoadData(int UserId,CMapleClient client)
        {
            CCharacter character = Sql.MySqlFactory.GetFactory.Query<CCharacter>().Where(a => a.Id == UserId).FirstOrDefault();

            if (character != null)
            {
                //设置上下文跟踪实体
                Sql.MySqlFactory.GetFactory.TrackEntity(character);
                client.CharacterInfo = new CMapleCharacter(character);
                foreach(CMapleInventory inv in client.CharacterInfo.inventory)
                {
                    inv.Load(character.Id);
                }
            }

            return character;
        }

        public static Dictionary<CCharacter,Dictionary<short,CItem>> ShowAllCharacter(int cid, int world)
        {
            Dictionary<CCharacter, Dictionary<short, CItem>> ret = new Dictionary<CCharacter, Dictionary<short, CItem>>();

            List<CCharacter> chrlist = Sql.MySqlFactory.GetFactory.Query<CCharacter>().Where(a => (a.Userid == cid && a.World == world)).ToList();

            foreach(CCharacter chr in chrlist)
            {
                //加载装备
                List<CItem> items = Sql.MySqlFactory.GetFactory.Query<CItem>().Where(a => a.Cid == chr.Id).ToList();
                Dictionary<short, CItem> itemlist = new Dictionary<short, CItem>();
                foreach (CItem item in items) {
                    itemlist.Add((short)item.Position, item);
                }

                ret.Add(chr,itemlist);
            }
            return ret;
        }


        public CMapleInventory GetMapleInventory(InventoryType type)
        {
            return inventory[(int)type];
        }


        public void SaveData()
        {
            //然后调用 Update 方法,这时只会更新被修改过的属性
            Sql.MySqlFactory.GetFactory.Update<CCharacter>(character);
        }

        /// <summary>
        /// 新建玩家
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool CreatorPlayer(int Userid,CMapleClient client, CCharacter character, Dictionary<byte, int> EquipList)
        {
            CCharacter ret = Common.Sql.MySqlFactory.GetFactory.Insert<CCharacter>(character);
            if (ret != null)
            {
                //自动创建道具栏
                Common.Sql.MySqlFactory.GetFactory.Insert<CInventoryslot>(new CInventoryslot
                {
                    Chid = ret.Id,
                    Elab = 32,
                    Equip = 32,
                    Setup = 32,
                    Use = 32,
                    Etc = 32,
                    Cash = 32,
                });

                client.CharacterInfo = new CMapleCharacter(ret);

                
                foreach(KeyValuePair<byte,int> itmeid in EquipList)
                {
                    CItem item = new CItem();
                    item.ItemId = itmeid.Value;
                    item.Position = itmeid.Key;
                    item.Cid = ret.Id;
                    client.CharacterInfo.GetMapleInventory(InventoryType.佩戴).addFromDB(item);                    
                }
                client.CharacterInfo.GetMapleInventory(InventoryType.佩戴).SevsDB();

                return true;
            }

            return false;
        }

    }
}
