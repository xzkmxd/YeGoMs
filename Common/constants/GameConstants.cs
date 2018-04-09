using System;
using System.Collections.Generic;
using System.Text;

namespace Common.constants
{
    public delegate void QuitServer();
    public class GameConstants
    {
        public static QuitServer _QuitServer;
        private static long[] exp = new long[201];


        /// <summary>
        /// 获取当前等级经验值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static long getExpNeededForLevel(int level)
        {
            if (level < 1 || level >= exp.Length)
            {
                return long.MaxValue;
            }
            return exp[level];
        }

        /// <summary>
        /// 加载经验值
        /// </summary>
        public static void LoadExp()
        {
            for (int i = 1; i <= 50; i++)
            {
                if (i <= 5)
                {
                    exp[i] = i * (i * i / 2 + 15);
                }
                else if (i > 5 && i <= 50)
                {
                    exp[i] = i * i / 3 * (i * i / 3 + 19);
                }
            }
            for (int i = 51; i <= 200; i++)
            {
                exp[i] = (long)((exp[i - 1]) * 1.0548);
            }
        }


    }

    public enum WroldName
    {
        蓝蜗牛,
        蘑菇仔,
        绿水灵,
        漂漂猪,
        小青蛇,
        红螃蟹,
        大海龟,
        章鱼怪,
        顽皮猴,
        星精灵,
        胖企鹅,
    }
}
