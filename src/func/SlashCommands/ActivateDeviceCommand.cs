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
        public const string ExpectedCommandType = "activate";

        private string _code;

        public ActivateDeviceCommand()
        {
        }

        public ActivateDeviceCommand(SlackSlashCommandRequest req) : base(req)
        {
            var args = req.Text?.Split(' ');
            if (args != null)
            {
                _code = args.FirstOrDefault(x => x.Length == RegisterDeviceCommand.CodeLength);
            }
        }

        public override async Task<SlackSlashCommandResponse> Execute(TraceWriter log)
        {
            if (string.IsNullOrEmpty(_code))
            {
                return new SlackSlashCommandResponse
                {
                    ResponseType = MessageResponseType.Ephemeral,
                    Text = "Invalid Device Code.",
                    Status = MessageResponseStatus.Failure,
                    SubStatus = MessageResponseSubStatus.Validation
                };
            }

            log.Verbose($"Attempting to register code {_code}");
            var command = new DeviceActivationCommand(_code, Request.TeamId, Request.TeamDomain,
                Request.ChannelId, Request.ChannelName, Request.UserId, Request.UserName,
                ConnectionFactory.Instance.DeviceRegistration);
            var result = await command.ExecuteAsync();

            switch (result.Status)
            {
                case ActivationStatus.Success:
                    log.Info($"Activated Device {_code}");
                    return new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.InChannel,
                        Text = "Got it! You are good to go."
                    };
                default:
                    log.Verbose($"Invalid Code {_code}");
                    return new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.Ephemeral,
                        Text = $"The code {_code} was not found.",
                        Status = MessageResponseStatus.Failure,
                        SubStatus = MessageResponseSubStatus.NotFound

                    };
            }
        }

        public override string CommandType => ExpectedCommandType;
    }
}
