using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using MusicBot.App;
using MusicBot.App.Commands;

namespace MusicBot.Functions
{
    public static class Activate
    {
        [FunctionName("activate-device")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "device/activate")] HttpRequestMessage req,
            TraceWriter log)
        {
            var slackRequest = new SlackSlashCommandRequest(await req.Content.ReadAsFormDataAsync());

            if (slackRequest.Token != Config.Instance.SlackVerificationToken)
            {
                log.Warning($"Unauthorized call to activate-device ({slackRequest.Token})");
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(slackRequest.Text) || slackRequest.Text.Trim().Length != RegisterDeviceCommand.CodeLength)
            {
                log.Verbose($"Invalid Code {slackRequest.Text.Trim()}");
                return req.CreateResponse(HttpStatusCode.BadRequest, new SlackSlashCommandResponse
                {
                    ResponseType = MessageResponseType.Ephemeral,
                    Text = "Invalid Code"
                });
            }

            log.Verbose($"Attempting to register code {slackRequest.Text.Trim()}");
            var command = new DeviceActivationCommand(slackRequest.Text.Trim(), ConnectionFactory.Instance.DeviceRegistration);
            var result = await command.ExecuteAsync();

            switch (result.Status)
            {
                case ActivationStatus.Success:
                    log.Info($"Activated Device {slackRequest.Text.Trim()}");
                    return req.CreateResponse(HttpStatusCode.OK, new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.Ephemeral,
                        Text = "Got it! You are good to go."
                    });
                default:
                    log.Verbose($"Invalid Code {slackRequest.Text.Trim()}");
                    return req.CreateResponse(HttpStatusCode.BadRequest, new SlackSlashCommandResponse
                    {
                        ResponseType = MessageResponseType.Ephemeral,
                        Text = "That code was not found."
                    });
            }
        }
    }
}