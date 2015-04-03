using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace FbChatApi
{
    public class FbLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public FbWebRequest WebRequest { get; set; }
        public MsgApi MsgApi { get; set; }
        public FbLogin(string email, string password)
        {
            Email = email;
            Password = password;
            WebRequest = new FbWebRequest();
            MsgApi = new MsgApi(WebRequest);
        }

        public async Task<bool> Login()
        {
            try
            {
                string val;
                var request = WebRequest.CreateGetRequest("https://www.facebook.com/");
                var response = await request.GetResponseAsync();
                var html = new HtmlDocument();
                html.Load(response.GetResponseStream());
                var doc = html.DocumentNode;
                var form = doc.QuerySelector("#login_form");
                var input = form.NextSibling;
                List<HtmlInput> l = new List<HtmlInput>();
                do
                {
                    if (input.Name == "input")
                    {
                        l.Add(new HtmlInput()
                        {
                            Name = input.GetAttributeValue("name", ""),
                            Value = input.GetAttributeValue("value", "")
                        });
                    }
                    input = input.NextSibling;
                } while (input != null);

                l.Add(new HtmlInput() { Name = "email", Value = Email });
                l.Add(new HtmlInput() { Name = "pass", Value = Password });
                l.Add(new HtmlInput() { Name = "default_persistent", Value = "1" });

                var connect = await WebRequest.CreatePostRequestAsync("https://www.facebook.com/login.php?login_attempt=1", l);
                var rep = connect.GetResponse();
                var page = string.Empty;
                using (var reader = new StreamReader(rep.GetResponseStream()))
                {
                    page = await reader.ReadToEndAsync();
                }

                var rand = new Random();
                var clientid = ((int)(rand.NextDouble() * 2147483648)).ToString("x");
                var cookies = WebRequest.Container.GetCookies(new Uri("https://www.facebook.com/"));
                var userId = cookies["c_user"].Value;
                var userChannel = "p_" + userId;
                List<HtmlInput> formL = new List<HtmlInput>()
                {
                    new HtmlInput(){Name = "channel",Value = userChannel},
                    new HtmlInput(){Name = "seq",Value = "0"},
                    new HtmlInput(){Name = "partition",Value = "-2"},
                    new HtmlInput(){Name = "clientid",Value = clientid},
                    new HtmlInput(){Name = "viewer_uid",Value = userId},
                    new HtmlInput(){Name = "state",Value = "active"},
                    new HtmlInput(){Name = "format",Value = "json"},
                    new HtmlInput(){Name = "idle",Value = "0"},
                    new HtmlInput(){Name = "cap",Value = "8"},
                };
                var fbDtsg = GetFrom(page, "name=\"fb_dtsg\" value=\"", "\"");
                var ttstamp = "";
                for (var i = 0; i < fbDtsg.Length; i++)
                {
                    ttstamp += fbDtsg.ToCharArray()[i];
                }
                ttstamp += '2';
                MsgApi.Clientid = clientid;
                MsgApi.FbDtsg = fbDtsg;
                MsgApi.Ttstamp = ttstamp;
                MsgApi.UserId = userId;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetFrom(string str, string startToken, string endToken)
        {
            var start = str.IndexOf(startToken, StringComparison.Ordinal) + startToken.Length;
            var lastHalf = str.Substring(start);
            var end = lastHalf.IndexOf(endToken, StringComparison.Ordinal);
            return lastHalf.Substring(0, end);
        }
    }

    public class HtmlInput
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
