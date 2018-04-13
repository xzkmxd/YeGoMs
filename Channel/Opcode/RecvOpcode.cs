using System;
using System.Collections.Generic;
using System.Text;

namespace Channel.Opcode
{
    public enum RecvOpcode : byte
    {
        登陆请求 = 6,
        频道更换 = 0x16,
        游戏聊天 = 31,

    }
}
