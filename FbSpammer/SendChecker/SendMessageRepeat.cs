using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbSpammer.SendChecker
{
    public class SendMessageRepeat:SendMessage
    {
        public TimeSpan SendEvery { get; set; }

    }
}
