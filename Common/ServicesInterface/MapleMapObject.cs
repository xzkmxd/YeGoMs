using Common.Buffer;
using Common.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServicesInterface
{

    public abstract class AnimatedMapleMapObject : CMapleMapObject
    {
        private byte m_stance;

        public byte GetStance()
        {
            return m_stance;
        }

        public void SetStance(byte stance)
        {
            m_stance = stance;
        }
    }

    public enum MapleMapObjectType
    {
        PLAYER,//玩家
        NPC,//NPC
        MONSTER,//怪物
        ITEM,//道具
        SUMMON,//召唤兽
        LOVE,//心
        MIST,//烟雾
        REACTOR,//反射堆
        HIRED_MERCHANT,//雇佣商人
    };

    /// <summary>
    /// 地图对象
    /// </summary>
    public abstract class CMapleMapObject
    {
        protected Point m_point;
        protected int m_objectId;
        public Point GetPoint()
        {
            return m_point;
        }

        public void SetPoint(Point pos)
        {
            m_point = pos;
        }

        public abstract MapleMapObjectType GetType();

        public int GetObjectId()
        {
            return m_objectId;
        }
        public void SetObjectId(int Id)
        {
            m_objectId = Id;
        }

        public abstract void SendDestroyData(CMapleClient c, MaplePakcet pakcet);

        public abstract void SendSpawnData(CMapleClient c, MaplePakcet pakcet);

    }

}
