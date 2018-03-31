﻿using Common.Buffer;
using Common.Client;
using Common.Global;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Handler
{
    public class MapleServerHandler : ChannelHandlerAdapter
    {
        /// <summary>
        /// 频道(-1商城.1~20频道,-2拍卖)
        /// </summary>
        public int channel = -1;
        public MapleServerHandler()
        {

        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            try
            {
                MapleBuffer buffer = new MapleBuffer((byte[])message);
                if (buffer.Available < 2)
                {
                    return;
                }

                MapleClient client = context.GetAttribute<MapleClient>(MapleClient.attributeKey).Get();
                if (client != null)
                {
                    short packetId = buffer.read<short>();


                    CommonGlobal.Run(packetId, buffer, client);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("错误:" + e);
            }
        }

        /// <summary>
        /// 游戏版本封包
        /// </summary>
        /// <param name="mapleVersion"></param>
        /// <param name="sendIv"></param>
        /// <param name="recvIv"></param>
        /// <returns></returns>
        public static MapleBuffer GetHello(short mapleVersion, byte[] sendIv, byte[] recvIv)
        {
            //using (MapleBuffer mapleBuffer = new MapleBuffer())
            //{
            MapleBuffer mapleBuffer = new MapleBuffer();
            mapleBuffer.add((short)0x0E);
            mapleBuffer.add<short>(mapleVersion);
            mapleBuffer.add(recvIv);
            mapleBuffer.add(sendIv);
            mapleBuffer.add(4);

            return mapleBuffer;
            //}
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
            System.Console.WriteLine("注册:" + context.Channel.RemoteAddress.ToString());
            byte[] ivRecv = { 70, 114, 122, 82 };
            context.Channel.WriteAndFlushAsync(ivRecv);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {

            System.Console.WriteLine("发现用户:" + context.Channel.RemoteAddress.ToString());
            //027配套

            //创建一个客户端
            byte[] ivRecv = { 70, 114, 122, 82 };
            byte[] ivSend = { 82, 48, 120, 115 };
            MapleClient client = new MapleClient(27, ivRecv, ivSend, context.Channel);
            context.Channel.WriteAndFlushAsync(GetHello(27, ivRecv, ivSend));


            //设置客户端
            context.GetAttribute<MapleClient>(MapleClient.attributeKey).Set(client);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            System.Console.WriteLine("连接异常:"+exception);
        }

        /// <summary>
        /// 用户断开连接
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine("用户:" + context.Channel.RemoteAddress.ToString() + "断开!");
        }
    }
}
