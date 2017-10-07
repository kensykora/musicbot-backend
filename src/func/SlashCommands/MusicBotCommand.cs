using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Host;

using MusicBot.Functions.Models;

namespace MusicBot.Functions.SlashCommands
{
    public abstract class MusicBotCommand
    {
        protected MusicBotCommand()
        {
            
        }

        protected MusicBotCommand(SlackSlashCommandRequest req)
        {
            Request = req;
            Args = req.Text?.Split(' ');
        }

        public abstract Task<SlackSlashCommandResponse> Execute(TraceWriter log);

        public SlackSlashCommandRequest Request { get; }

        public abstract string CommandType { get; }

        public string[] Args { get; }
    }
}
