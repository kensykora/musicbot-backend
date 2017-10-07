using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using MusicBot.Functions.Models;

namespace MusicBot.Functions.Test.Extensions
{
    public static class SlackSlashCommandRequestExtensions
    {
        public static FormUrlEncodedContent GetFormContent(this SlackSlashCommandRequest req)
        {
            return new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", req.Token),
                new KeyValuePair<string, string>("team_id", req.TeamId),
                new KeyValuePair<string, string>("team_domain", req.TeamDomain),
                new KeyValuePair<string, string>("enterprise_id", req.EnterpriseId),
                new KeyValuePair<string, string>("enterprise_name", req.EnterpriseName),
                new KeyValuePair<string, string>("channel_id", req.ChannelId),
                new KeyValuePair<string, string>("channel_name", req.ChannelName),
                new KeyValuePair<string, string>("user_id", req.UserId),
                new KeyValuePair<string, string>("user_name", req.UserName),
                new KeyValuePair<string, string>("command", req.Command),
                new KeyValuePair<string, string>("text", req.Text),
                new KeyValuePair<string, string>("response_url", req.ResponseUrl)

        });
        }
    }
}
