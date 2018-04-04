using Common.Client;
using Common.Tools;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Buffer
{
    public class MapleEncoder : MessageToByteEncoder<MaplePakcet>
    {
        protected override void Encode(IChannelHandlerContext context, MaplePakcet message, IByteBuffer output)
        {
            using (message)
            {
                CMapleClient client = context.GetAttribute<CMapleClient>(CMapleClient.attributeKey).Get();
                if (client == null)
                {
                    output.WriteBytes(message.ToArray());
                }
                else
                {
                    //加密
                    using (MapleBuffer buffer = new MapleBuffer())
                    {
                        byte[] header = new byte[4];
                        client.m_SendIv.GetHeaderToClient(message.ToArray().Length, header);
                        client.m_SendIv.Transform();
                        buffer.add(header);
                        buffer.add(message.ToArray());

                        System.Console.WriteLine("发送封包:{0}", HexTool.toString(message.ToArray()));
                        output.WriteBytes(buffer.ToArray());
                    }
                }
            }            
        }
    }
}
