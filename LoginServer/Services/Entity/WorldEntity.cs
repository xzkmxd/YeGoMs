using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.ServicesInterface;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;

namespace LoginServer.Services.Entity
{
    //public class WorldEntity : Common.ServicesInterface.WorldInterface
    //{
    //    private static List<WorldInfo> m_WorldList = new List<WorldInfo>();

    //    //TODO:获取世界链表(完成)
    //    public static List<WorldInfo> GetWorld()
    //    {
    //        return m_WorldList;
    //    }

    //    //TODO:向世界添加频道(完成)
    //    public Task<bool> AddChannelInfo(int WorldId, ChannelInfo info)
    //    {
    //        foreach (WorldInfo world in m_WorldList)
    //        {
    //            if (world.WorldId == WorldId)
    //            {
    //                world.channelInfo.Add(info);
    //                return Task.FromResult(true);
    //            }
    //        }
    //        return Task.FromResult(false);
    //    }

    //    //TODO:获取世界(完成)
    //    public Task<WorldInfo> GetWorldInfo(int WorldId)
    //    {
    //        foreach (WorldInfo world in m_WorldList)
    //        {
    //            if (world.WorldId == WorldId)
    //            {
    //                return Task.FromResult(world);
    //            }
    //        }
    //        return Task.FromResult<WorldInfo>(null);
    //    }

    //    //TODO:注册世界(完成)
    //    public Task<WorldInfo> RegisterWorld(WorldInfo info)
    //    {
    //        if (m_WorldList.Count >= System.Enum.GetNames(typeof(Common.constants.WroldName)).Length)
    //        {
    //            return Task.FromResult<WorldInfo>(null);
    //        }
    //        else
    //        {
    //            info.WorldId = m_WorldList.Count;
    //            info.Name = System.Enum.GetName(typeof(Common.constants.WroldName), info.WorldId);
    //            m_WorldList.Add(info);
    //            return Task.FromResult(info);
    //        }
    //    }

    //    //TODO:删除世界(完成)
    //    public Task<WorldInfo> RemoveWorld(int WorldId)
    //    {
    //        foreach (WorldInfo world in m_WorldList)
    //        {
    //            if (world.WorldId == WorldId)
    //            {
    //                if (m_WorldList.Remove(world))
    //                {
    //                    return Task.FromResult<WorldInfo>(world);
    //                }
    //            }
    //        }
    //        return Task.FromResult<WorldInfo>(null);
    //    }
    //}
}
