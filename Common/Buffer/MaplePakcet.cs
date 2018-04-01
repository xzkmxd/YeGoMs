using Common.Attribute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Common.Buffer
{
    /// <summary>
    /// 
    /// </summary>
    public class MaplePakcet : IDisposable
    {
        private MemoryStream m_stream;
        public MaplePakcet(byte[] buffer)
        {            
            //获取运行函数
            MethodBase @base = new StackTrace().GetFrame(1).GetMethod();
            System.Attribute attribute = @base.GetCustomAttribute(typeof(PacketHead), true);
            if (attribute != null)
            {
                m_stream = new MemoryStream();
                m_stream.WriteByte(byte.Parse(((PacketHead)attribute).Head.ToString()));
                m_stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                m_stream = new MemoryStream(buffer);
            }

        }

        public void Dispose()
        {
            if (m_stream != null)
            {
                m_stream.Dispose();
            }

            m_stream = null;

        }

        public byte[] ToArray()
        {
            return m_stream.ToArray();
        }
    }
}
