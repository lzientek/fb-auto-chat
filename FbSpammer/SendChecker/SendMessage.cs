using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbSpammer.SendChecker
{
    public class SendMessage
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime LastSend { get; set; }
        public async Task<bool> Send()
        {
            if (FbChatConnector.FbChat == null || !FbChatConnector.FbChat.IsConnected)
                return false;
            var result = await FbChatConnector.FbChat.JsApiConnector.SendMessage(Message, UserId);
            LastSend = DateTime.Now;
            return true;
        }
    }
}
