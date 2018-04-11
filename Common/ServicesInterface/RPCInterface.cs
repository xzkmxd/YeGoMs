using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServicesInterface
{
    /// <summary>
    /// 服务器列表中的频道信息
    /// </summary>
    public class ChannelInfo
    {
        /// <summary>
        /// 在线人数
        /// </summary>
        public int Players { get; set; }

        /// <summary>
        /// 区名
        /// </summary>
        public string Name { get; set; }

        public int Id { get; set; }

    }

    /// <summary>
    /// 世界信息
    /// </summary>
    public class WorldInfo
    {
        /// <summary>
        /// 频道列表
        /// </summary>
        public List<ChannelInfo> channelInfo = new List<ChannelInfo>();

        /// <summary>
        /// 世界名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 世界ID
        /// </summary>
        public int WorldId { get; set; }
    }

    /// <summary>
    /// 返回结果类型
    /// </summary>
    public class RetType
    {
        public int Port { get; set; }
        public int UID { get; set; }
        public int Error { get; set; }
    }

    [RpcServiceBundle]
    public interface WorldInterface
    {
        /// <summary>
        /// 注册世界
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<WorldInfo> RegisterWorld(WorldInfo info);

        /// <summary>
        /// 删除世界
        /// </summary>
        /// <param name="WorldId"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<WorldInfo> RemoveWorld(int WorldId);

        /// <summary>
        /// 向某个世界增加频道信息
        /// </summary>
        /// <param name="WorldId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<bool> AddChannelInfo(int WorldId, ChannelInfo info);

        /// <summary>
        /// 获取世界对象
        /// </summary>
        /// <param name="WorldId"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<WorldInfo> GetWorldInfo(int WorldId);

        [RpcService(IsWaitExecution = true)]
        Task<bool> RemoveChannel(int WorldId, int ChannelId);
    }

    public class WorldEntity : Common.ServicesInterface.WorldInterface
    {
        private static List<WorldInfo> m_WorldList = new List<WorldInfo>();

        //TODO:获取世界链表(完成)
        public static List<WorldInfo> GetWorld()
        {
            return m_WorldList;
        }

        //TODO:向世界添加频道(完成)
        public Task<bool> AddChannelInfo(int WorldId, ChannelInfo info)
        {
            Console.WriteLine("频道开始注册!!");
            foreach (WorldInfo world in m_WorldList)
            {
                if (world.WorldId == WorldId)
                {
                    world.channelInfo.Add(info);
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }

        //TODO:向世界删除频道(完成)
        public Task<bool> RemoveChannel(int WorldId,int ChannelId)
        {
            foreach(ChannelInfo info in m_WorldList[WorldId].channelInfo)
            {
                if(info.Id == ChannelId)
                {
                    Console.WriteLine("{0}世界-{1}频道注销成功!");
                    return Task.FromResult(m_WorldList[WorldId].channelInfo.Remove(info));
                }
            }

            return Task.FromResult(false);
        }

        //TODO:获取世界(完成)
        public Task<WorldInfo> GetWorldInfo(int WorldId)
        {
            foreach (WorldInfo world in m_WorldList)
            {
                if (world.WorldId == WorldId)
                {
                    return Task.FromResult(world);
                }
            }
            return Task.FromResult<WorldInfo>(null);
        }

        //TODO:注册世界(完成)
        public Task<WorldInfo> RegisterWorld(WorldInfo info)
        {
            if (m_WorldList.Count >= System.Enum.GetNames(typeof(Common.constants.WroldName)).Length)
            {
                return Task.FromResult<WorldInfo>(null);
            }
            else
            {
                info.WorldId = m_WorldList.Count;
                info.Name = System.Enum.GetName(typeof(Common.constants.WroldName), info.WorldId);
                m_WorldList.Add(info);
                Console.WriteLine("{0}世界-{1}服务器 注册成功!", info.WorldId, info.Name);
                return Task.FromResult(info);
            }
        }

        //TODO:删除世界(完成)
        public Task<WorldInfo> RemoveWorld(int WorldId)
        {
            foreach (WorldInfo world in m_WorldList)
            {
                if (world.WorldId == WorldId)
                {
                    if (m_WorldList.Remove(world))
                    {
                        Console.WriteLine("{0}世界-{1}服务器 注销成功!", world.WorldId, world.Name);
                        return Task.FromResult<WorldInfo>(world);
                    }
                }
            }
            return Task.FromResult<WorldInfo>(null);
        }
    }

    [RpcServiceBundle]
    public interface ChannelInterface
    {
        /// <summary>
        /// 注册频道
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<RetType> RegisterChannel(ChannelInfo info);

        /// <summary>
        /// 删除频道
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<bool> RemoveChannel(int ChannelId);

        /// <summary>
        /// 获取频道对象
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        /// 
        [RpcService(IsWaitExecution = true)]
        Task<ChannelInfo> GetChannelInfo(int ChannelId);
    }

    public class ChannelEntity : ChannelInterface
    {
        public static WorldInfo worldEntity;
        public static WorldInterface world;
        public static List<ChannelInfo> m_ChannelList = new List<ChannelInfo>();
        public Task<ChannelInfo> GetChannelInfo(int ChannelId)
        {
            return Task.FromResult(m_ChannelList[ChannelId]);
        }

        public Task<RetType> RegisterChannel(ChannelInfo info)
        {
            RetType ret = new RetType();

            if (m_ChannelList.Count >= 20)
            {
                ret.Error = -1;
            }
            else
            {
                ret.Port = 7575 + m_ChannelList.Count;
                ret.UID = m_ChannelList.Count;
                m_ChannelList.Add(info);
            }
            Console.WriteLine("进行注册频道{0}", ret.UID);
            Task.Run(async () =>
            {
                await world.AddChannelInfo(worldEntity.WorldId, info);
            }).Wait();
            return Task.FromResult(ret);
        }

        public Task<bool> RemoveChannel(int ChannelId)
        {
            m_ChannelList.RemoveAt(ChannelId);
            Console.WriteLine("{0}频道注销成功!", ChannelId);
            Task.Run(async () =>
            {
                await world.RemoveChannel(worldEntity.WorldId, ChannelId);
            }).Wait();
            return Task.FromResult(true);
        }
    }



}
