﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using MusicBot.App;
using MusicBot.App.Commands;
using MusicBot.Functions.Models;
using MusicBot.Functions.SlashCommands;

using Newtonsoft.Json;

namespace MusicBot.Functions
{
    public static class SlashCommand
    {
        public const string Path = "slashcommand";

        [FunctionName(Path)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Path)] HttpRequestMessage req,
            TraceWriter log = null)
        {
            if (log == null)
            {
                log = new TraceMonitor();
            }
            var slackRequest = new SlackSlashCommandRequest(await req.Content.ReadAsFormDataAsync());

            if (slackRequest.Token != Config.Instance.SlackVerificationToken)
            {
                log.Warning($"Unauthorized call to command ({slackRequest.Token})");
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(slackRequest.Command))
            {
                log.Verbose($"Invalid Command {slackRequest.Command}");
                return req.CreateResponse(HttpStatusCode.OK, new SlackSlashCommandResponse
                {
                    ResponseType = MessageResponseType.Ephemeral,
                    Text = "Invalid Command"
                });
            }

            try
            {
                var command = MusicBotCommandFactory.GetCommand(slackRequest);
                if (command == null)
                {
                    return req.CreateResponse(HttpStatusCode.OK, new SlackSlashCommandResponse()
                    {
                        Status = MessageResponseStatus.Failure,
                        SubStatus = MessageResponseSubStatus.UnknownCommand,
                        ResponseType = MessageResponseType.Ephemeral,
                        Text = $"Unknown command {slackRequest.Text?.Split(' ').FirstOrDefault()}".Trim()
                    });
                }
                return req.CreateResponse(await command.Execute(log));
            }
            catch (Exception ex)
            {
                log.Error("Unexpected Error", ex);
                return req.CreateResponse(HttpStatusCode.OK, new SlackSlashCommandResponse
                {
                    ResponseType = MessageResponseType.Ephemeral,
                    Text = "Oops, an unexpected error occurred. We'll look into it.",
                    Status = MessageResponseStatus.Failure,
                    SubStatus = MessageResponseSubStatus.Internal
                });
            }

        }
    }
}