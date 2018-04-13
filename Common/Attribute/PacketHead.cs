using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Attribute
{
    public class PacketHead : System.Attribute
    {
        public short Head { get; set; }
        public string Text { set; get; }
        public int Progress { set; get; }
        public Type Type { get; set; }

        /// <summary>
        /// 封包头特性
        /// </summary>
        /// <param name="head">包头类型</param>
        /// <param name="text">处理说明</param>
        public PacketHead(short head,string text)
        {
            Head = head;
            Text = text;
        }

        public PacketHead(object head,Type type,int _progress = 0)
        {
            System.Byte Vaul = (System.Byte)Enum.Parse(type, head.ToString());
            Head = byte.Parse(Vaul.ToString());
            Text = head.ToString();
            Progress = _progress;
            Type = type;
        }

    }
}
