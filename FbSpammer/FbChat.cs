using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FbAutoChat.Core;



namespace FbSpammer
{
    public static class FbChatConnector
    {
        private static FbChatApi.FbChatApi _fbChatApi;

        public static FbChatApi.FbChatApi FbChat
        {
            get { return _fbChatApi; }
            set { _fbChatApi = value; }
        }

        public static FbChatApi.FbChatApi Load(User user)
        {
            return FbChat = new FbChatApi.FbChatApi(user.Mail, user.Password);
        }
    }
}
