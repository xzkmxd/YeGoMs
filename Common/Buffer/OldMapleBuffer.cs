using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Buffer
{
    /// <summary>
    /// 老版Buffer
    /// </summary>
    public class OldMapleBuffer : IDisposable
    {
        private MemoryStream memoryStream;
        Queue<byte> queue = new Queue<byte>();
        long position = 0;

        public OldMapleBuffer()
        {

        }

        public OldMapleBuffer(short head)
        {
            add<short>(head);
        }

        public OldMapleBuffer(byte[] buffer)
        {
            foreach (byte bytes in buffer)
            {
                queue.Enqueue(bytes);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private void write(byte b)
        {
            position++;
            queue.Enqueue(b);
        }

        private byte readByte()
        {
            if (position <= 0 || queue.Count == 0)
            {
                throw (new BufferException("容器过小.无法操作"));
            }
            else
            {
                position--;
                return queue.Dequeue();
            }
        }

        byte[] str2ASCII(String xmlStr)
        {
            return Encoding.GetEncoding("GBK").GetBytes(xmlStr);
        }

        string Ascii2Str(byte[] buf)
        {
            return Encoding.GetEncoding("GBK").GetString(buf);
        }



        public void add<T>(T bufferx)
        {
            Type type = bufferx.GetType();
            switch (type.ToString())
            {
                case "System.Int32"://int
                    write((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 16) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 24) & 0xFF));
                    break;
                case "System.Int64"://long
                    write((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 16) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 24) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 32) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 40) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 48) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 56) & 0xFF));
                    break;
                case "System.String"://string
                    byte[] retbytes = str2ASCII(bufferx.ToString());
                    write((byte)(byte.Parse(retbytes.Length.ToString()) & 0xFF));
                    write((byte)((byte.Parse(retbytes.Length.ToString()) >> 8) & 0xFF));
                    foreach (byte b in retbytes)
                    {
                        write(b);
                    }
                    break;
                case "System.Int16"://short
                    write((byte)(byte.Parse(bufferx.ToString()) & 0xFF));
                    write((byte)((byte.Parse(bufferx.ToString()) >> 8) & 0xFF));
                    break;
                case "System.Byte":
                    break;
                default:
                    int aa = 0;
                    break;
            }
        }

        public T read<T>()
        {
            T type = default(T);
            object ret = null;
            if (type == null)
            {
                int byte1 = readByte();
                int byte2 = readByte();
                int lent = (short)((byte2 << 8) + byte1);
                byte[] Texts = new byte[lent];
                for (int i = 0; i < lent; i++)
                {
                    Texts[i] = queue.Dequeue();
                }

                ret = Ascii2Str(Texts);
            }
            else
            {
                switch (type.GetType().ToString())
                {
                    case "System.Int32":
                        {
                            int byte1 = readByte();
                            int byte2 = readByte();
                            int byte3 = readByte();
                            int byte4 = readByte();
                            ret = (int)((byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1);
                            break;
                        }
                    case "System.Int64":
                        {
                            long byte1 = readByte();
                            long byte2 = readByte();
                            long byte3 = readByte();
                            long byte4 = readByte();
                            long byte5 = readByte();
                            long byte6 = readByte();
                            long byte7 = readByte();
                            long byte8 = readByte();
                            ret = (long)(byte8 << 56) + (byte7 << 48) + (byte6 << 40) + (byte5 << 32) + (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
                            break;
                        }
                    case "System.Int16":
                        {
                            int byte1 = readByte();
                            int byte2 = readByte();
                            ret = (short)((byte2 << 8) + byte1);
                            break;
                        }
                    case "System.Byte":
                        {
                            ret = readByte();
                            break;
                        }
                }
            }

            return (T)ret;

        }


        public void Dispose()
        {
            queue.Clear();
            queue = null;
        }

        public byte[] ToArry()
        {
            return queue.ToArray();
        }
    }
}
