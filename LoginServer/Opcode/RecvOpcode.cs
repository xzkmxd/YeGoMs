using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Opcode
{
    enum RecvOpcode : byte
    {
        帐号登录 = 1,
        性别选择,
        大区选择,
        请求角色,
        开始游戏,
        登陆请求,
        检测名字,
        未知0x8,
        帐号检查,
        帐号注册,
        创建角色,
        未知0xC,
        心跳包,
        客户端错误,
    }
}
