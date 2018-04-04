using Common.Buffer;
using Common.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Handle
{
    public abstract class HandlerInterface
    {
        public abstract void Handle(MapleBuffer mapleBuffer,CMapleClient client);
    }


}
