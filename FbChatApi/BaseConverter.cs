using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbChatApi
{
    public static class BaseConverter
    {
        static string _alphaDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ToBase(this int value, UInt32 toBase)
        {
            string result = "";
            UInt32 val = (uint) value;
            do
            {
                UInt32 digitValue = val % toBase;
                result += _alphaDigits[(int)digitValue];
                val /= toBase;
            } while (val != 0);
            result = ReverseString(result);
            return result;
        }

        public static UInt32 StringToUInt(this string s, UInt32 toBase)
        {
            UInt32 result = 0;

            for (int i = 0; i < s.Length; i++)
            {
                result *= toBase;
                result += (UInt32)_alphaDigits.IndexOf(s[i]);
            }
            return result;
        }

        public static string ReverseString(string s)
        {
            string result = "";

            for (int i = 0; i < s.Length; i++)
                result = s.Substring(i, 1) + result;
            return result;
        }
    }
}
