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
        public MessageConnector MessageConnector { get; set; }
        public UserConnector UserConnector { get; set; }

        public FbChatApi(string email, string password)
        {
            IsConnected = false;
            Email = email;
            Password = password;
            WebRequest = new FbWebRequest();
            MessageConnector = new MessageConnector(WebRequest);
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

                UserConnector.LoadSomeFriends(page);
                IsConnected = true;

                //do a lot of pull request
                var l2 = new List<HtmlInput>(){
                    new HtmlInput(){Name ="grammar_version",Value = grammar_version},
                    new HtmlInput(){Name ="__user",Value = MessageConnector.UserId},
                    new HtmlInput(){Name ="__a",Value = "1"},
                    new HtmlInput(){Name ="__req",Value = (2).ToBase(36)},
                };
                var nullStateRequest =
                     WebRequest.CreateGetRequest("https://www.facebook.com/ajax/browse/null_state.php", l2);
                rep = await nullStateRequest.GetResponseAsync();


                var l3 = new List<HtmlInput>(){
                    new HtmlInput(){Name ="grammar_version",Value = grammar_version},
                    new HtmlInput(){Name ="__user",Value = MessageConnector.UserId},
                    new HtmlInput(){Name ="__a",Value = "1"},
                    new HtmlInput(){Name ="__req",Value = (3).ToBase(36)},
                    new HtmlInput(){Name ="__rev",Value = MessageConnector.Rev},
                    new HtmlInput(){Name ="reason",Value = "6"},
                    new HtmlInput(){Name ="fb_dtsg",Value = MessageConnector.FbDtsg},
                };
                var recoRequest =
                     WebRequest.CreateGetRequest("https://www.facebook.com/ajax/presence/reconnect.php", l3);
                rep = await recoRequest.GetResponseAsync();

                //pull requests
                var userChannel = "p_" + UserConnector.UserId;
                var l4 = new List<HtmlInput>(){
                        new HtmlInput {Name = "channel",Value = userChannel},
                        new HtmlInput {Name = "seq",Value = "0"},
                        new HtmlInput {Name = "partition",Value = "-2"},
                        new HtmlInput {Name = "clientid",Value = MessageConnector.Clientid},
                        new HtmlInput {Name = "viewer_uid",Value = UserConnector.UserId},
                        new HtmlInput {Name = "state",Value = "active"},
                        new HtmlInput {Name = "format",Value = "json"},
                        new HtmlInput {Name = "idle",Value = "0"},
                        new HtmlInput {Name = "cap",Value = "8"},
                };
                var pullRequest =
                     WebRequest.CreateGetRequest("https://0-edge-chat.facebook.com/pull", l4);
                rep = await pullRequest.GetResponseAsync();
                using (StreamReader reader = new StreamReader(rep.GetResponseStream(), Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    l4.Add(new HtmlInput() { Name = "sticky_token", Value = Helper.GetFrom(json, "sticky\":\"", "\",") });
                    l4.Add(new HtmlInput() { Name = "sticky_pool", Value = Helper.GetFrom(json, "pool\":\"", "\"}") });
                    l4.Add(new HtmlInput() { Name = "wtc", Value = "0,0,0.000,0,0" });
                }
                var pullRequest2 =
                     WebRequest.CreateGetRequest("https://0-edge-chat.facebook.com/pull", l4);
                rep = await pullRequest2.GetResponseAsync();

                //await SyncRequest();

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
                new HtmlInput() {Name = "__user", Value = MessageConnector.UserId},
                new HtmlInput() {Name = "__a", Value = "1"},
                new HtmlInput() {Name = "__req", Value = (4).ToBase(36)},
                new HtmlInput() {Name = "__rev", Value = MessageConnector.Rev},
                new HtmlInput() {Name = "lastSync", Value = (DateTime.Now.ToTimeStamp() - 60).ToString()},
            };
            var syncRequest = WebRequest.CreateGetRequest("https://www.facebook.com/notifications/sync/", l5);
            await syncRequest.GetResponseAsync();

            //thread sync request
            var l6 = new List<HtmlInput>()
            {
                new HtmlInput() {Name = "__user", Value = MessageConnector.UserId},
                new HtmlInput() {Name = "__a", Value = "1"},
                new HtmlInput() {Name = "__req", Value = (5).ToBase(36)},
                new HtmlInput() {Name = "__rev", Value = MessageConnector.Rev},
                new HtmlInput() {Name = "ttstamp", Value = MessageConnector.Ttstamp},
                new HtmlInput() {Name = "client", Value = "mercury"},
                new HtmlInput() {Name = "folders[0]", Value = "inbox"},
                new HtmlInput() {Name = "last_action_timestamp", Value = "0"},
                new HtmlInput() {Name = "fb_dtsg", Value = MessageConnector.FbDtsg},
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
            MessageConnector.Clientid = clientid;
            MessageConnector.FbDtsg = fbDtsg;
            MessageConnector.Ttstamp = ttstamp;
            MessageConnector.UserId = userId;
            MessageConnector.Rev = rev;
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