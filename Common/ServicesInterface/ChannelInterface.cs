using ProtoBuf;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServicesInterface
{

    public class ChannelModel
    {
        //地图(玩家集合)
        public Dictionary<int, List<object>> m_Players = new Dictionary<int, List<object>>();

        public void AddPlayer(int MapId,object player)
        {
            m_Players[MapId].Add(player);
        }

        public void Remove(int MapId,object player)
        {
            m_Players[MapId].Remove(player);
        }

        public ChannelModel(int Mapid)
        {
            m_Players.Add(Mapid, new List<object>());
        }

    }

    /// <summary>
    /// 向世界服务器进行注册频道
    /// </summary>
    /// 
    [RpcServiceBundle]
    public interface ChannelInterface
    {
        /// <summary>
        /// 获取频道对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ChannelModel> GetChannel(int id);
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
        ///
        [RpcService(IsWaitExecution = true)]
        Task<ReturnState> RegisterServices(ChannelModel Channel);

    }

    /// <summary>
    /// 频道注册返回值
    /// </summary>
    public class ReturnState
    {
        public short Index { get; set; }

        public short Port { get; set; }

        public int Error { get; set; }
    }


    public class ChannelServices : ChannelInterface
    {
        public static Dictionary<int, ChannelModel> dictionary = new Dictionary<int, ChannelModel>();
        public int Cout = 10;

        public Task<ChannelModel> GetChannel(int id)
        {
            return Task.FromResult(dictionary[id]);
        }


        public Task<ReturnState> RegisterServices(ChannelModel Channel)
        {
            System.Console.WriteLine("注册!!!!");
            ReturnState posint = new ReturnState();
            if (dictionary.Count >= Cout)
            {
                posint.Error = -1;
                return Task.FromResult(posint);
            }
            else
            {
                posint.Index = (short)dictionary.Count;
                posint.Port = (short)(7575 + dictionary.Count);
                dictionary.Add(dictionary.Count, Channel);
                System.Console.WriteLine("频道:{0}注册成功!", dictionary.Count);
            }

            return Task.FromResult(posint);
        }
    }
}
