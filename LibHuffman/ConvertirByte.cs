using System;
using System.Collections.Generic;
using System.Text;

namespace LibCompress
{
    class ConvertirByte
    {
        static Encoding e = Encoding.GetEncoding("iso-8859-1");
        public static byte[] Convertir(string text)
        {
            return e.GetBytes(text);
        }

        public static byte Convertir(char elchar)
        {
            char[] convert = new char[1];
            convert[0] = elchar;
            var bytes = e.GetBytes(convert);
            return bytes[0];
        }

        public static string ConvertirP(byte[] bytes)
        {
            return e.GetString(bytes);
        }
    }
}
