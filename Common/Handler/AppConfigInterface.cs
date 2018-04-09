using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Handler
{
    public interface AppConfigInterface
    {
        short GetPort();
        string GetWorldAddress();

        /// <summary>
        /// 该方法只有频道才存在
        /// </summary>
        /// <returns></returns>
        short GetId();
        
    }
}
