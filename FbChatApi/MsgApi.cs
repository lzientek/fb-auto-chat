using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbChatApi
{
    public class MsgApi
    {
        private int _reqCounter = 1;
        public FbWebRequest WebRequest { get; set; }
        public string UserId { get; set; }
        public string Clientid { get; set; }
        public string Ttstamp { get; set; }
        public string FbDtsg { get; set; }


        public MsgApi(FbWebRequest webRequest)
        {
            WebRequest = webRequest;
        }

        public async void SendMessage(string msg, string threadId)
        {
            var timestamp = DateTime.Now.ToTimeStamp();
            var d = new DateTime();
            var form = new List<HtmlInput>
                    {
                        new HtmlInput{Name="client",Value="mercury"},
                        new HtmlInput{Name="__user",Value=UserId},
                        new HtmlInput{Name="fb_dtsg",Value=FbDtsg},
                        new HtmlInput{Name="ttstamp",Value=Ttstamp},
                        new HtmlInput{Name="__req",Value=(_reqCounter++).ToBase(36)},
                        new HtmlInput{Name="message_batch[0][action_type]",Value="ma-type:user-generated-message"},
                        new HtmlInput{Name="message_batch[0][author]",Value=string.Format("fbid:{0}", UserId)},
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
                        new HtmlInput{Name="message_batch[0][thread_fbid]",Value=threadId},
                        new HtmlInput{Name="message_batch[0][has_attachment]",Value="false"},
                    };
            var req = await WebRequest.CreatePostRequestAsync("https://www.facebook.com/ajax/mercury/send_messages.php", form);
            var result = await req.GetResponseAsync();
        }


    }
}
