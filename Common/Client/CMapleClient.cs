using Common.Buffer;
using Common.Client.SQL;
using Common.Cryptography;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Client
{

    /// <summary>
    /// 帐号登陆状态
    /// </summary>
    public enum LOGINSTATE
    {
        OFFLIEN,//离线
        LANDFALL,//登录中
        INGAME,//游戏中
    }


    public class CMapleClient
    {
        public static AttributeKey<CMapleClient> attributeKey = AttributeKey<CMapleClient>.NewInstance("Client");
        public MapleCipher m_SendIv;
        public MapleCipher m_RecvIv;
        //客户端套接字
        public IChannel m_Session;
        public int DecoderState = -1;
        public CUser UserInfo { get; set; }
        public CMapleCharacter CharacterInfo { get; set; }


        public CMapleClient(short Version, byte[] Riv,byte[] Siv, IChannel channel)
        {
            var userkey = new byte[] //europe maplestory key
            {
                0x13, 0x00, 0x00, 0x00,
                0x08, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00,
                0xB4, 0x00, 0x00, 0x00,
                0x1B, 0x00, 0x00, 0x00,
                0x0F, 0x00, 0x00, 0x00,
                0x33, 0x00, 0x00, 0x00,
                0x52, 0x00, 0x00, 0x00
            };
            var aes = new AesCipher(userkey);
            //设置加密与解密
            m_RecvIv = new MapleCipher(Version, Riv, aes, CipherType.Decrypt);
            m_SendIv = new MapleCipher(Version, Riv, aes, CipherType.Encrypt);
            m_Session = channel;
            DecoderState = -1;
        }

        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="datat"></param>
        public void SendDatat(MaplePakcet datat)
        {
            using (datat)
            {
                m_Session.WriteAndFlushAsync(datat);
            }
        }

    }
}
