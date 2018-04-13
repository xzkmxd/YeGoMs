using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Client;

namespace ChannelServer.Map
{
    public class MapleMapFactory
    {
        private static MapleMapFactory factory;
        private Dictionary<int, MapModel> m_MapList = new Dictionary<int, MapModel>();

        public static void Init()
        {
            ///初始化地图工厂模式
            ///            
            factory = new MapleMapFactory();
            for (int i = 0; i < 1000; i++)
            {
                factory.m_MapList.Add(i, new MapModel());
            }
        }

        public MapModel GetMap(int MapId)
        {
            return m_MapList[MapId];
        }

        public static MapleMapFactory MapFactory
        {
            get
            {
                return factory;
            }
        }


        //private static Dictionary<int, List<CMapleCharacter>> m_Map = new Dictionary<int, List<CMapleCharacter>>();

        //public static void AddPlayer(int MapId,CMapleCharacter chr)
        //{
        //    Console.WriteLine("{0}玩家进入地图:{1}", chr.character.Name, chr.character.MapId);
        //    if (!m_Map.ContainsKey(MapId))
        //    {
        //        m_Map.Add(MapId, new List<CMapleCharacter>());
        //        m_Map[MapId].Add(chr);
        //    }
        //    else
        //    {
        //        m_Map[MapId].Add(chr);
        //    }
        //}

        //public static void RemovePlayer(int MapId, CMapleCharacter chr)
        //{
        //    Console.WriteLine("{0}玩家退出地图:{1}", chr.character.Name, chr.character.MapId);
        //    m_Map[MapId].Remove(chr);

        //}
    }
}
