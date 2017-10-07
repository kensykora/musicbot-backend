using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Host;

using MusicBot.Functions.Models;

namespace MusicBot.Functions.SlashCommands
{
    public class PlayCommand : MusicBotCommand
    {
        public PlayCommand()
        {
            
        }

        public PlayCommand(SlackSlashCommandRequest req) : base(req)
        {
        }

        public override Task<SlackSlashCommandResponse> Execute(TraceWriter log)
        {
            return Task.FromResult(new SlackSlashCommandResponse()
            {
                ResponseType = MessageResponseType.InChannel,
                Text = "Now Playing"
            });
        }

        public override string CommandType => "play";
    }
}
