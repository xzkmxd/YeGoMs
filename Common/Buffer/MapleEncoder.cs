using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Buffer
{
    public class MapleEncoder : MessageToByteEncoder<MapleBuffer>
    {
        protected override void Encode(IChannelHandlerContext context, MapleBuffer message, IByteBuffer output)
        {
            byte[] input = message.ToArray();
            output.WriteBytes(input);
        }
    }
}
