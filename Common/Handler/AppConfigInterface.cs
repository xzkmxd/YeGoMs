using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Handler
{
    public interface AppConfigInterface
    {
        short GetPort();
        string GetWorldAddress();
        
    }
}
