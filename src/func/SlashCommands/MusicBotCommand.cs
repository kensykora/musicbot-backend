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
        public abstract string ExpectedCommandType { get; }

        protected MusicBotCommand(SlackSlashCommandRequest req)
        {
            Request = req;

            CommandType = req.Command;
            Args = req.Text?.Split(' ');

            if (!ExpectedCommandType.Equals(CommandType, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Incorrect command type for this command");
            }
        }

        public abstract Task<SlackSlashCommandResponse> Execute(TraceWriter log);

        public SlackSlashCommandRequest Request { get; }

        public string CommandType { get; }

        public string[] Args { get; }
    }
}
