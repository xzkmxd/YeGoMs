using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Attribute
{
    public class CommanAttribute : System.Attribute
    {
        /// <summary>
        /// 命令名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { set; get; }

        public int Parameters { get; set; }

        /// <summary>
        /// 封包头特性
        /// </summary>
        /// <param name="head">包头类型</param>
        /// <param name="text">处理说明</param>
        public CommanAttribute(string CommanName, string Explain = "", int Para = 1)
        {
            Name = CommanName;
            this.Explain = Explain;
            Parameters = Para;
        }

    }
}
