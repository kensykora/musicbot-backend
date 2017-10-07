using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Host;

using MusicBot.App;
using MusicBot.App.Commands;
using MusicBot.Functions.Models;

namespace MusicBot.Functions.SlashCommands
{
    public class ActivateDeviceCommand : MusicBotCommand
    {
        public string Code { get; }

        public ActivateDeviceCommand(SlackSlashCommandRequest req) : base(req)
        {
            if (Args.Length != 1)
            {
                throw new InvalidCommandException("Device command not supplied");
            }

            Code = Args[0];
            if (Code.Trim().Length != RegisterDeviceCommand.CodeLength)
            {
                throw new InvalidCommandException("Invalid Code");
            }
        }

        public override string ExpectedCommandType => "activate";
        public override async Task<SlackSlashCommandResponse> Execute(TraceWriter log)
        {
            log.Verbose($"Attempting to register code {Code}");
            var command = new DeviceActivationCommand(Code, Request.TeamId, Request.TeamDomain,
                Request.ChannelId, Request.ChannelName, Request.UserId, Request.UserName,
                ConnectionFactory.Instance.DeviceRegistration);
            var result = await command.ExecuteAsync();

            switch (result.Status)
            {
                case ActivationStatus.Success:
                    log.Info($"Activated Device {Request.Text.Trim()}");
                    return new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.InChannel,
                        Text = "Got it! You are good to go."
                    };
                default:
                    log.Verbose($"Invalid Code {Request.Text.Trim()}");
                    return new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.Ephemeral,
                        Text = "That code was not found."
                    };
            }
        }
    }
}
