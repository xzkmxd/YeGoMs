using Common.Buffer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer.Packet
{
    public class LoginPakcet
    {

        /// <summary>
        /// 验证版本
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHello(short mapleVersion, byte[] sendIv, byte[] recvIv)
        {
            using (MapleBuffer mapleBuffer = new MapleBuffer())
            {
                mapleBuffer.add((short)0x0E);
                mapleBuffer.add<short>(mapleVersion);
                mapleBuffer.add(recvIv);
                mapleBuffer.add(sendIv);
                mapleBuffer.add(4);

                return mapleBuffer.ToArray();
            }
        }

    }
}
