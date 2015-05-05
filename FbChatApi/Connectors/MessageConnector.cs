using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FbChatApi
{
    public class MessageConnector
    {
        private int _reqCounter = 6;
        public FbWebRequest WebRequest { get; set; }
        public string UserId { get; set; }
        public string Clientid { get; set; }
        public string Ttstamp { get; set; }
        public string FbDtsg { get; set; }
        public string Rev { get; set; }

        public MessageConnector(FbWebRequest webRequest)
        {
            WebRequest = webRequest;
        }

        public async Task<WebResponse> SendMessage(string msg, string threadId)
        {
            var timestamp = DateTime.Now.ToTimeStamp();
            var d = new DateTime();
            double rand = (new Random()).NextDouble();
            var form = new List<HtmlInput>
                    {
                        new HtmlInput{Name="client",Value="mercury"},
                        new HtmlInput{Name="__a",Value="1"},
                        new HtmlInput{Name="__rev",Value=Rev},
                        new HtmlInput{Name="__user",Value=UserId},
                        new HtmlInput{Name="__req",Value=(_reqCounter++).ToBase(36)},
                        new HtmlInput{Name="fb_dtsg",Value=FbDtsg},
                        new HtmlInput{Name="ttstamp",Value=Ttstamp},
                        new HtmlInput{Name="message_batch[0][action_type]",Value="ma-type:user-generated-message"},
                        new HtmlInput{Name="message_batch[0][thread_id]",Value=null},
                        new HtmlInput{Name="message_batch[0][author]",Value=string.Format("fbid:{0}", UserId)},
                        new HtmlInput{Name="message_batch[0][author_email]",Value=null},
                        new HtmlInput{Name="message_batch[0][coordinates]",Value=null},
                        new HtmlInput{Name="message_batch[0][timestamp]",Value=timestamp.ToString()},
                        new HtmlInput{Name="message_batch[0][timestamp_absolute]",Value="Today"},
                        new HtmlInput{Name="message_batch[0][timestamp_relative]",
                            Value=string.Format("{0}:{1}", d.Hour,Helper.PadZeros(d.Minute.ToString()))},
                        new HtmlInput{Name="message_batch[0][timestamp_time_passed]",Value="0"},
                        new HtmlInput{Name="message_batch[0][is_unread]",Value="false"},
                        new HtmlInput{Name="message_batch[0][is_cleared]",Value="false"},
                        new HtmlInput{Name="message_batch[0][is_forward]",Value="false"},
                        new HtmlInput{Name="message_batch[0][is_filtered_content]",Value="false"},
                        new HtmlInput{Name="message_batch[0][is_spoof_warning]",Value="false"},
                        new HtmlInput{Name="message_batch[0][source]",Value="source:chat:web"},
                        new HtmlInput{Name="message_batch[0][source_tags][0]",Value="source:chat"},
                        new HtmlInput{Name="message_batch[0][body]",
                            Value=!string.IsNullOrEmpty(msg) ? msg : ""},
                        new HtmlInput{Name="message_batch[0][html_body]",Value="false"},
                        new HtmlInput{Name="message_batch[0][ui_push_phase]",Value="V3"},
                        new HtmlInput{Name="message_batch[0][status]",Value="0"},
                        new HtmlInput{Name="message_batch[0][message_id]",
                            Value=Helper.GenerateMessageId(Clientid)},
                        new HtmlInput{Name="message_batch[0][manual_retry_cnt]",Value="0"},
                        new HtmlInput{Name="message_batch[0][has_attachment]",Value="false"},
                        new HtmlInput{Name="message_batch[0][client_thread_id]",Value=string.Format("user:{0}", threadId)},
                        //new HtmlInput{Name="message_batch[0][specific_to_list][0]",Value=string.Format("fbid:{0}", threadId)},
                        //new HtmlInput{Name="message_batch[0][specific_to_list][1]",Value=string.Format("fbid:{0}", UserId)},

                        new HtmlInput{Name="message_batch[0][thread_fbid]",Value=threadId},
                        new HtmlInput{Name="message_batch[0][signatureID]",Value=((int)Math.Floor(rand * 2147483648)).ToBase(16)},
                    };
            var req = await WebRequest.CreatePostRequestAsync("https://www.facebook.com/ajax/mercury/send_messages.php", form);
            var result = await req.GetResponseAsync();
            using (StreamReader reader = new StreamReader(result.GetResponseStream(), Encoding.UTF8))
            {
                reader.ReadToEnd();
            }
            return result;
        }


    }
}
