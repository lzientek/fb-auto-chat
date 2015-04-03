using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbChatApi
{
    public static class Helper
    {
        public static int ToTimeStamp(this DateTime date)
        {
            DateTime date1 = new DateTime(1970, 1, 1);
            TimeSpan ts = new TimeSpan(date.Ticks - date1.Ticks);
            return (int)ts.TotalSeconds;
        }
        public static string PadZeros(string val, int len = 2)
        {
            while (val.Length < len) val = "0" + val;
            return val;
        }

        public static string GenerateMessageId(string clientID)
        {
            var k = DateTime.Now.ToTimeStamp();
            var rand = new Random();
            var l = Math.Floor(rand.NextDouble() * 4294967295);
            var m = clientID;
            return string.Format("<{0}:{1}-{2}@mail.projektitan.com>", k, l, m);
        }
    }
}
