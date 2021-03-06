﻿using System;
using System.Collections.Generic;
using System.IO;
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
            return string.Format("<{0}:{1}-{2}@mail.projektitan.com>", k, l, clientID);
        }
        public static string GetFrom(string str, string startToken, string endToken)
        {
            var start = str.IndexOf(startToken, StringComparison.Ordinal) + startToken.Length;
            var lastHalf = str.Substring(start);
            var end = lastHalf.IndexOf(endToken, StringComparison.Ordinal);
            return lastHalf.Substring(0, end);
        }

        public static FbSmallUser ToFbSmallUser(this FbUser fbUser)
        {
            return new FbSmallUser
            {
                name = fbUser.Name,
                firstName = fbUser.First_Name,
                id = fbUser.Id,
                vanity= fbUser.Username,
                gender = fbUser.Gender,
                thumbSrc = fbUser.ProfilePicture
            };
        }
    }
}

