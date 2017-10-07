using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MusicBot.Functions.Models;
using MusicBot.Functions.Test.Extensions;

namespace MusicBot.Functions.Test.Context
{
    public class SlashCommandTestsContext : BaseTestsContext
    {
        public const string StandardSlackPath = SlashCommand.Path;

        public string StandardToken => Config.Instance.SlackVerificationToken;

        public string InvalidToken => "DEF234";

        public SlackSlashCommandRequest StandardSlackSlashCommandRequest => new SlackSlashCommandRequest()
        {
            ChannelId = "C2147483705",
            ChannelName = "test",
            EnterpriseId = "E0001",
            EnterpriseName = "Globular Construct Inc",
            UserId = "U2147483697",
            UserName = "Steve",
            TeamId = "T0001",
            TeamDomain = "ostusa.com",
            Token = StandardToken,
            ResponseUrl = "https://hooks.slack.com/commands/1234/5678"
        };

        public HttpRequestMessage GetStandardSlackHttpRequestMessage(SlackSlashCommandRequest requestIs = null, bool useDefaults = true)
        {
            if (requestIs == null && useDefaults)
            {
                requestIs = StandardSlackSlashCommandRequest;
            }

            var result =
                new HttpRequestMessage(HttpMethod.Post, new Uri("https://apihost/api/" + StandardSlackPath))
                {
                    Content = requestIs.GetFormContent(),
                };
            result.SetConfiguration(new HttpConfiguration());

            return result;
        }
    }
}
