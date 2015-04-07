using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FbChatApi
{
    public class UserConnector
    {
        public string UserId { get; set; }
        public Dictionary<string,FbSmallUser> Friends { get; set; } 
        public FbWebRequest WebRequest { get; set; }

        public UserConnector(FbWebRequest webRequest)
        {
            WebRequest = webRequest;
            Friends = new Dictionary<string, FbSmallUser>();
        }

        public void LoadSomeFriends(string htmlStartPage)
        {
            var val = Helper.GetFrom(htmlStartPage, "[\"ShortProfiles\",\"setMulti\",[],[", "]]");
            Friends = JsonConvert.DeserializeObject<Dictionary<string, FbSmallUser>>(val);
        }

        public IEnumerable<FbSmallUser> GetFriendsAsList()
        {
            return Friends.Where(pair => pair.Key != UserId).Select(pair => pair.Value);
        } 

        public async Task<FbUser> GetActualUserAsync()
        {
            return await GetUserAsync(UserId);
        }
        public async Task<FbUser> GetUserAsync(string idOrName)
        {
            var req = WebRequest.CreateGraphGetRequest(idOrName);
            var rep = await req.GetResponseAsync();
            using (StreamReader reader = new StreamReader(rep.GetResponseStream()))
            {
                string val = await reader.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<FbUser>(val);
                return json;
            }

            
        }


    }
}