using Common.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Buffer
{

    public class BufferException : System.Exception
    {
        public BufferException(string Msg) : base(Msg)
        {

        }
    }

    /// <summary>
    /// 新版Buffer
    /// </summary>
    public class MapleBuffer : IDisposable
    {
        public const int DefaultBufferSize = 32;

        #region Writer
        private MemoryStream m_stream;
        private bool m_disposed;
        #endregion

        #region Reader
        private int m_index;
        #endregion

        #region Writer
        public int Position
        {
            get
            {
                return (int)m_stream.Position;
            }
            set
            {
                if (value <= 0)
                    throw new BufferException("Value less than 1");

                m_stream.Position = value;
            }
        }
        public bool Disposed
        {
            get
            {
                return m_disposed;
            }
        }

        public MapleBuffer()
        {
            m_stream = new MemoryStream(DefaultBufferSize);
            m_disposed = false;
        }


        public MapleBuffer(byte[] buffer)
        {
            m_stream = new MemoryStream(buffer);
            m_index = 0;
        }

        public byte[] ToArray()
        {
            ThrowIfDisposed();
            return m_stream.ToArray();
        }


        #region 写Byte类型数据
        private void WriteByte(byte value = 0)
        {
            ThrowIfDisposed();
            m_stream.WriteByte(value);
        }
        #endregion

        #region 字符串转换
        private byte[] str2ASCII(String xmlStr)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB2312").GetBytes(xmlStr);
        }

        private string Ascii2Str(byte[] buf)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding("GB2312").GetString(buf);
        }
        #endregion

        #region 封包接口
        public void add<T>(T bufferx)
        {
            Type type = bufferx.GetType();

            switch (type.ToString())
            {
                case "System.Int32"://int
                    WriteByte((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 16) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 24) & 0xFF));
                    break;
                case "System.Int64"://long
                    WriteByte((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 16) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 24) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 32) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 40) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 48) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 56) & 0xFF));
                    break;
                case "System.String"://string
                    byte[] retbytes = str2ASCII(bufferx.ToString());
                    WriteByte((byte)(byte.Parse(retbytes.Length.ToString()) & 0xFF));
                    WriteByte((byte)((byte.Parse(retbytes.Length.ToString()) >> 8) & 0xFF));
                    foreach (byte b in retbytes)
                    {
                        WriteByte(b);
                    }
                    break;
                case "System.Int16"://short
                    WriteByte((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    WriteByte((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    break;
                case "System.Boolean":
                    WriteByte((Boolean.Parse(bufferx.ToString())) ?  (byte)1 : (byte)0);
                    break;
                case "System.Byte":
                    WriteByte(byte.Parse(bufferx.ToString()));
                    break;
                default:
                    throw new BufferException("Error No Type:" + type.ToString());
            }
        }
        #endregion

        public void add(byte[] buffer)
        {
            foreach (byte s in buffer)
            {
                WriteByte(s);
            }
        }

        public int Available
        {
            get
            {
                return m_stream.ToArray().Length - m_index;
            }
        }

        public void CheckLength(int length)
        {
            if (m_index + length > m_stream.Length || length < 0)
                throw new BufferException("Not enough space");
        }

        private byte ReadByte()
        {
            CheckLength(1);
            return m_stream.ToArray()[m_index++];//m_buffer[m_index++];
        }

        public T read<T>()
        {
            T type = default(T);
            object ret = null;
            if (type == null)
            {
                int byte1 = ReadByte();
                int byte2 = ReadByte();
                int lent = (short)((byte2 << 8) + byte1);
                byte[] Texts = new byte[lent];
                for (int i = 0; i < lent; i++)
                {
                    Texts[i] = ReadByte();
                }

                ret = Ascii2Str(Texts);
            }
            else
            {
                switch (type.GetType().ToString())
                {
                    case "System.Int32":
                        {
                            int byte1 = ReadByte();
                            int byte2 = ReadByte();
                            int byte3 = ReadByte();
                            int byte4 = ReadByte();
                            ret = (int)((byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1);
                            break;
                        }
                    case "System.Int64":
                        {
                            long byte1 = ReadByte();
                            long byte2 = ReadByte();
                            long byte3 = ReadByte();
                            long byte4 = ReadByte();
                            long byte5 = ReadByte();
                            long byte6 = ReadByte();
                            long byte7 = ReadByte();
                            long byte8 = ReadByte();
                            ret = (long)(byte8 << 56) + (byte7 << 48) + (byte6 << 40) + (byte5 << 32) + (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
                            break;
                        }
                    case "System.Int16":
                        {
                            int byte1 = ReadByte();
                            int byte2 = ReadByte();
                            ret = (short)((byte2 << 8) + byte1);
                            break;
                        }
                    case "System.Byte":
                        {
                            ret = ReadByte();
                            break;
                        }
                }
            }

            return (T)ret;

        }

        private void ThrowIfDisposed()
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }


        ~MapleBuffer()
        {
            m_disposed = true;

            if (m_stream != null)
            {
                m_stream.Dispose();
            }

            m_stream = null;
        }
        void IDisposable.Dispose()
        {

            m_disposed = true;

            if (m_stream != null)
            {
                m_stream.Dispose();
            }

            m_stream = null;

        }

        public override string ToString()
        {
            return HexTool.toString(ToArray());
        }

        public MaplePakcet GetPakcet()
        {
            return new MaplePakcet(ToArray());
        }

        #endregion
    }
}
