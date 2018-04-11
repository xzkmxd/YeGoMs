using Common.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Services
{
#if 测试5
    public class WroldFace : ChannelInterface
    {
        public static WroldModel model = new WroldModel();
        /// <summary>
        /// 最大二十个频道
        /// </summary>
        public const int MaxChannelCout = 20;

        /// <summary>
        /// 频道对象
        /// </summary>
        public List<ChannelModel> m_ChanneldList;

        public WroldFace()
        {
            if (m_ChanneldList == null)
            {
                m_ChanneldList = new List<ChannelModel>();
            }
        }

        public Task<ChannelModel> GetChannel(int id)
        {
            return Task.FromResult(m_ChanneldList[id]);
        }

        public Task<ReturnState> RegisterServices(ChannelModel Channel)
        {
            if (GetWroldModel().m_ChanneldList == null)
            {
                GetWroldModel().m_ChanneldList = new List<ChannelModel>();
            }
            Channel.Id = GetWroldModel().m_ChanneldList.Count;
            ReturnState ret = new ReturnState();
            ret.ChannelModel = Channel;
            if (GetWroldModel().m_ChanneldList.Count >= MaxChannelCout)
            {
                ret.Error = -1;//注册完毕.
            }
            else
            {
                ret.Index = (short)GetWroldModel().m_ChanneldList.Count;
                ret.Port = (short)(7575 + GetWroldModel().m_ChanneldList.Count);
                GetWroldModel().m_ChanneldList.Add(Channel);
                System.Console.WriteLine("频道:{0}注册成功! {1}", Channel.Id, GetWroldModel().m_ChanneldList.Count);
            }

            return Task.FromResult(ret);

        }

        public static WroldModel GetWroldModel(int Id)
        {
            return WroldServices.m_WroldList[Id];
        }

        public static WroldModel GetWroldModel()
        {
            return model;
        }

        public Task<bool> RemoveWorld(int id)
        {
            foreach (ChannelModel wr in GetWroldModel(ID).m_ChanneldList)
            {
                if (wr.Id == id)
                {
                    Console.WriteLine("{0} 频道注销成功!", wr.Id);
                    GetWroldModel(ID).m_ChanneldList.Remove(wr);
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }

        public string Name { get; set; }

        public int ID { get; set; }
    }
#endif
}
