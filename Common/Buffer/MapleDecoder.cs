using Common.Client;
using Common.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Buffer
{
    /// <summary>
    /// 简单解密器
    /// </summary>
    public class MapleDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            CMapleClient client = context.GetAttribute<CMapleClient>(CMapleClient.attributeKey).Get();
            if(client.DecoderState == -1)
            {
                //检测长度
                if(input.WriterIndex >= 4)
                {
                    int packetHeader = input.ReadInt();
                    client.DecoderState = MapleCipher.getPacketLength(packetHeader);//MapleAESOFB.getPacketLength(packetHeader);
                    int aaa = 0;
                    client.m_RecvIv.Transform();
                }
                else
                {
                    return;
                }
            }


            if(input.ReadableBytes >= client.DecoderState)
            {
                client.DecoderState = -1;
                //IntPtr DecoderState = context.GetAttribute<IntPtr>(MapleClient.DecoderState).Get();
                //获取数据长度,创建一个空数组
                byte[] DecodePakcet = new byte[input.ReadableBytes];

                //读取正常数据区域
                input.ReadBytes(DecodePakcet);
                //解密成功后返回数据
                output.Add(DecodePakcet);
            }
        }
    }
}
