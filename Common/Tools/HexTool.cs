using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tools
{
    public class HexTool
    {
        private static char[] HEX = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public static String toString(byte byteValue)
        {
            int tmp = byteValue << 8;
            char[] retstr = new char[] { HEX[(tmp >> 12) & 0x0F], HEX[(tmp >> 8) & 0x0F] };
            return String.Join(" ", retstr);
        }

        public static String toString(int intValue)
        {
            if(intValue >= 0 && intValue <= 9)
            {
                return "0" + intValue;
            }
            return Convert.ToString(intValue,16).ToUpper();
        }

        public static String toString(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 1)
            {
                return "";
            }
            StringBuilder hexed = new StringBuilder();
            foreach (byte aByte in bytes)
            {
                hexed.Append(toString((int)aByte));
                hexed.Append(' ');
            }
            return hexed.ToString(0, hexed.Length - 1);
        }

    }
}
