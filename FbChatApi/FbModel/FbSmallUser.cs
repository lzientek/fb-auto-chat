using System.Collections.Generic;
using System.Linq;

namespace FbChatApi
{
    public class FbSmallUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string vanity { get; set; }
        public string thumbSrc { get; set; }
        public string uri { get; set; }
        public string gender { get; set; }
        public string type { get; set; }
        public bool is_friend { get; set; }
        public object mThumbSrcSmall { get; set; }
        public object mThumbSrcLarge { get; set; }
        public object dir { get; set; }
        public List<string> searchTokens { get; set; }
        public string alternateName { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}