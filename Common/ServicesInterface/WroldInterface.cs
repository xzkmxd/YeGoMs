using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServicesInterface
{
    public class WroldModel
    {
        //一个世界存在多个频道
        public List<ChannelServices> m_ChannelList = new List<ChannelServices>();

        public static string GetName(int id)
        {
            return System.Enum.GetName(typeof(constants.WroldName), id);
        }

        public string Name { get; set; }

        public int ID { get; set; }
    }

    [RpcServiceBundle]
    public interface WroldInterface
    {
        [RpcService(IsWaitExecution = true)]
        //世界名称,
        Task<WroldModel> RegisterServices(WroldModel wrold);

        [RpcService(IsWaitExecution = true)]
        Task<WroldModel> GetWrold(int id);
        Task<int> GetId(WroldModel wrold);

        Task<int> GetSize();

        [RpcService(IsWaitExecution = true)]
        Task<bool> RemoveWorld(int id);
    }


    /// <summary>
    /// 世界服务实例
    /// </summary>
    public class WroldServices : WroldInterface
    {
        public static List<WroldModel> m_WroldList = new List<WroldModel>();

        /// <summary>
        /// 根据对象获取编号
        /// </summary>
        /// <param name="wrold"></param>
        /// <returns></returns>
        public Task<int> GetId(WroldModel wrold)
        {
            for(int i = 0; i < m_WroldList.Count; i++)
            {
                if(m_WroldList[i] == wrold)
                {
                    return Task.FromResult(i);
                }
            }

            return Task.FromResult(-1);
        }

        public Task<int> GetSize()
        {
            return Task.FromResult(m_WroldList.Count-1);
        }

        /// <summary>
        /// 根据ID获取世界对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WroldModel> GetWrold(int id)
        {
            return Task.FromResult(m_WroldList[id]);
        }

        /// <summary>
        /// 注册世界对象
        /// </summary>
        /// <param name="wrold"></param>
        /// <returns></returns>
        /// 

        public Task<WroldModel> RegisterServices(WroldModel wrold)
        {
            wrold.ID = m_WroldList.Count;
            wrold.Name = WroldModel.GetName(wrold.ID);
            m_WroldList.Add(wrold);
            Console.WriteLine("{0} 世界 - {1} 注册成功!", wrold.ID, wrold.Name);
            return Task.FromResult(wrold);
        }

        public Task<bool> RemoveWorld(int id)
        {     
            foreach(WroldModel wr in m_WroldList)
            {
                if(wr.ID == id)
                {
                    Console.WriteLine("{0} 世界 - {1} 注销世界成功!", wr.ID, WroldModel.GetName(wr.ID));
                    m_WroldList.Remove(wr);
                    return Task.FromResult(true);
                }
            }            
            return Task.FromResult(false);
        }
    }
}
