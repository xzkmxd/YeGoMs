using System;
using System.Collections.Generic;
using System.Text;

namespace Channel.Opcode
{
    public enum SendOpcode:byte
    {
        道具信息 = 0x17,
        更新道具,
        更新能力值,
        技能效果状态,
        取消技能效果状态,
        未知0X1C,
        更新技能,
        未知0X1E,
        人气反馈,
        人物基础信息,
        显示小纸条,
        使用缩地石,
        未知0X23,
        角色信息,
        组队操作,
        好友列表,
        时空门,
        服务器公告,


        进入游戏=0x2B,
        进入商城,
        聊天信息=0x43,


    }
}
