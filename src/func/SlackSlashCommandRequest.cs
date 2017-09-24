using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MusicBot.Functions
{
    public class SlackSlashCommandRequest
    {
        public SlackSlashCommandRequest(NameValueCollection req)
        {
            Token = req.Get("token");
            TeamId = req.Get("team_id");
            TeamDomain = req.Get("team_domain");
            EnterpriseId = req.Get("enterprise_id");
            EnterpriseName = req.Get("enterprise_name");
            ChannelId = req.Get("channel_id");
            ChannelName = req.Get("channel_name");
            UserId = req.Get("user_id");
            UserName = req.Get("user_name");
            Command = req.Get("command");
            Text = req.Get("text");
            ResponseUrl = req.Get("response_url");
        }

        public string Token { get; set; }
        public string TeamId { get; set; }
        public string TeamDomain { get; set; }
        public string EnterpriseId { get; set; }
        public string EnterpriseName { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Command { get; set; }
        public string Text { get; set; }
        public string ResponseUrl { get; set; }
    }
}