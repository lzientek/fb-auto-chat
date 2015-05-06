using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace FbChatApi
{
    public class FbChatApi
    {
        private string grammar_version;

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsConnected { get; set; }
        public FbWebRequest WebRequest { get; set; }
        public JsApiConnector JsApiConnector { get; set; }
        public UserConnector UserConnector { get; set; }

        public FbChatApi(string email, string password)
        {
            IsConnected = false;
            Email = email;
            Password = password;
            WebRequest = new FbWebRequest();
            JsApiConnector = new JsApiConnector(WebRequest);
            UserConnector = new UserConnector(WebRequest);

        }

        public async Task<bool> Login()
        {
            try
            {
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
                        l.Add(new HtmlInput
                        {
                            Name = input.GetAttributeValue("name", ""),
                            Value = input.GetAttributeValue("value", "")
                        });
                    }
                    input = input.NextSibling;
                } while (input != null);

                l.Add(new HtmlInput { Name = "email", Value = Email });
                l.Add(new HtmlInput { Name = "pass", Value = Password });
                l.Add(new HtmlInput { Name = "default_persistent", Value = "1" });

                var connect = await WebRequest.CreatePostRequestAsync("https://www.facebook.com/login.php?login_attempt=1", l);
                var rep = await connect.GetResponseAsync();
                string page;
                using (var reader = new StreamReader(rep.GetResponseStream()))
                {
                    page = await reader.ReadToEndAsync();
                }

                GetValuesFromPage(page);
                await JsApiConnector.SetLogin(Email, Password);
                UserConnector.LoadSomeFriends(page);
                IsConnected = true;

                OnConnectionEnd();
                return IsConnected;
            }
            catch (Exception e)
            {
                OnConnectionEnd();
                return IsConnected;
            }
        }

        private async Task SyncRequest()
        {
//sync request
            var l5 = new List<HtmlInput>()
            {
                new HtmlInput() {Name = "__user", Value = JsApiConnector.UserId},
                new HtmlInput() {Name = "__a", Value = "1"},
                new HtmlInput() {Name = "__req", Value = (4).ToBase(36)},
                new HtmlInput() {Name = "__rev", Value = JsApiConnector.Rev},
                new HtmlInput() {Name = "lastSync", Value = (DateTime.Now.ToTimeStamp() - 60).ToString()},
            };
            var syncRequest = WebRequest.CreateGetRequest("https://www.facebook.com/notifications/sync/", l5);
            await syncRequest.GetResponseAsync();

            //thread sync request
            var l6 = new List<HtmlInput>()
            {
                new HtmlInput() {Name = "__user", Value = JsApiConnector.UserId},
                new HtmlInput() {Name = "__a", Value = "1"},
                new HtmlInput() {Name = "__req", Value = (5).ToBase(36)},
                new HtmlInput() {Name = "__rev", Value = JsApiConnector.Rev},
                new HtmlInput() {Name = "ttstamp", Value = JsApiConnector.Ttstamp},
                new HtmlInput() {Name = "client", Value = "mercury"},
                new HtmlInput() {Name = "folders[0]", Value = "inbox"},
                new HtmlInput() {Name = "last_action_timestamp", Value = "0"},
                new HtmlInput() {Name = "fb_dtsg", Value = JsApiConnector.FbDtsg},
            };
            var threadsyncRequest =
                await WebRequest.CreatePostRequestAsync("https://www.facebook.com/ajax/mercury/thread_sync.php", l6);
            await threadsyncRequest.GetResponseAsync();
        }

        public event ConnectionTerminated ConnectionEnd;

        private void OnConnectionEnd()
        {
            var handler = ConnectionEnd;
            if (handler != null) handler(this, null);
        }

        private void GetValuesFromPage(string page)
        {
            var rand = new Random();
            var clientid = ((int)(rand.NextDouble() * 2147483648)).ToString("x");
            var cookies = WebRequest.Container.GetCookies(new Uri("https://www.facebook.com/"));
            var userId = cookies["c_user"].Value;

            var fbDtsg = Helper.GetFrom(page, "name=\"fb_dtsg\" value=\"", "\"");
            var rev = Helper.GetFrom(page, "revision\":", ",");
            var ttstamp = "";
            for (var i = 0; i < fbDtsg.Length; i++)
            {
                ttstamp += ((int)fbDtsg[i]).ToString();
            }
            ttstamp += '2';
            JsApiConnector.Clientid = clientid;
            JsApiConnector.FbDtsg = fbDtsg;
            JsApiConnector.Ttstamp = ttstamp;
            JsApiConnector.UserId = userId;
            JsApiConnector.Rev = rev;
            UserConnector.UserId = userId;
            grammar_version = Helper.GetFrom(page, "grammar_version\":\"", "\"");
        }
    }
    public delegate void ConnectionTerminated(object sender, EventArgs args);

    public class HtmlInput
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

//var userChannel = "p_" + userId;
//List<HtmlInput> formL = new List<HtmlInput> //p-e inutile
//{
//    new HtmlInput {Name = "channel",Value = userChannel},
//    new HtmlInput {Name = "seq",Value = "0"},
//    new HtmlInput {Name = "partition",Value = "-2"},
//    new HtmlInput {Name = "clientid",Value = clientid},
//    new HtmlInput {Name = "viewer_uid",Value = userId},
//    new HtmlInput {Name = "state",Value = "active"},
//    new HtmlInput {Name = "format",Value = "json"},
//    new HtmlInput {Name = "idle",Value = "0"},
//    new HtmlInput {Name = "cap",Value = "8"},
//};