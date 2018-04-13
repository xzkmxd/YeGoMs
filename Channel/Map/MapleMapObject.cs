using ChannelServer.Packet;
using Common.Client;
using Common.ServicesInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelServer.Map
{

    /// <summary>
    /// 地图模型
    /// </summary>
    public class MapModel
    {
        private static Dictionary<MapleMapObjectType, Dictionary<int, CMapleMapObject>> m_MapList = new Dictionary<MapleMapObjectType, Dictionary<int, CMapleMapObject>>();
        /// <summary>
        /// 玩家对象链表
        /// </summary>
        private Dictionary<int, CMapleCharacter> m_Players = new Dictionary<int, CMapleCharacter>();

        /// <summary>
        /// 增加玩家
        /// </summary>
        /// <param name="chr"></param>
        public void AddPlayer(CMapleCharacter chr)
        {
            Console.WriteLine("{0}玩家进入地图:{1}", chr.character.Name, chr.character.MapId);
            //发送召唤玩家
            m_Players.Add(chr.character.Id, chr);
            foreach (KeyValuePair<int, CMapleCharacter> player in m_Players)
            {
                Console.WriteLine("玩家:{0} 进入地图{1}", player.Value.character.Name, chr.character.MapId);
                player.Value.SendSpawnData(player.Value.client, PlayerPakcet.SpawnPlayer(chr));
            }
            
        }

        /// <summary>
        /// 删除玩家
        /// </summary>
        /// <param name="chr"></param>
        public void RemovePlayer(CMapleCharacter chr)
        {
            Console.WriteLine("{0}玩家退出地图:{1}", chr.character.Name, chr.character.MapId);
            m_Players.Remove(chr.character.Id);
            foreach (KeyValuePair<int, CMapleCharacter> player in m_Players)
            {
                Console.WriteLine("玩家:{0} 退出地图{1}", player.Value.character.Name, chr.character.MapId);
                player.Value.SendSpawnData(player.Value.client, PlayerPakcet.RemovePlayer(chr.character.Id));
            }
        }



        /// <summary>
        /// 增加地图对象
        /// </summary>
        /// <param name="object"></param>
        public static void AddObject(CMapleMapObject @object)
        {
            if (!m_MapList.ContainsKey(@object.GetType()))
            {
                m_MapList.Add(@object.GetType(), new Dictionary<int, CMapleMapObject>());
            }

            m_MapList[@object.GetType()].Add(@object.GetObjectId(), @object);
        }

        /// <summary>
        /// 获取地图对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        public static CMapleMapObject GetObjec(MapleMapObjectType type, int Uid)
        {
            //Liqn语句查询对象.
            var ret = (from s in m_MapList[type]
                       where s.Key == Uid
                       select s.Value).First();

            return ret;
        }


        /// <summary>
        /// 获取玩家
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public  CMapleCharacter GetPlayer(int uid)
        {
            return m_Players[uid];
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="mapObject"></param>
        public static void RemoveObject(CMapleMapObject mapObject)
        {
            var groupList = (from s in m_MapList[mapObject.GetType()]
                             where s.Key.Equals(mapObject.GetType()) && s.Value.GetObjectId() == mapObject.GetObjectId()
                             select s).First().Value;
            //删除对应的UID对象
            m_MapList[mapObject.GetType()].Remove(mapObject.GetObjectId());
        }
    }
}
