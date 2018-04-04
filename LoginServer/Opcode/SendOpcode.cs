using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Opcode
{
    public enum SendOpcode:byte
    {
        登录状态 = 0x1,
        性别选择,
        性别反馈,
        服务器状态,
        服务器列表,
        人物列表,
        服务器IP,
        检测角色名,
        显示注册 = 0x9,
        检测帐号,
        注册帐号,
        增加人物,
        未知0xC,
        频道更换 = 0xE,
        心跳包 = 0xF,

    }
}
