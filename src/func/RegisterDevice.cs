using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using MusicBot.App;
using MusicBot.App.Commands;
using MusicBot.Functions.Models;

using Newtonsoft.Json;

namespace MusicBot.Functions
{
    public static class RegisterDevice
    {
        [FunctionName("device")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = null)] HttpRequestMessage req, TraceWriter log)
        {
            var body = await req.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(body))
                return req.CreateResponse(HttpStatusCode.BadRequest, "DeviceId is required");

            var registration =
                JsonConvert.DeserializeObject<DeviceRegistrationRequest>(await req.Content.ReadAsStringAsync());

            if (!registration.DeviceId.HasValue)
            {
                log.Error("Invalid Request - DeviceId is required.");
                return req.CreateResponse(HttpStatusCode.BadRequest, "DeviceId is required");
            }

            log.Info($"Put Device - {registration.DeviceId.Value}");

            var command =
                new RegisterDeviceCommand(registration.DeviceId.Value, ConnectionFactory.Instance.DeviceRegistration, ConnectionFactory.Instance.IoTHubClient);
            var result = await command.ExecuteAsync();

            return req.CreateResponse(HttpStatusCode.OK, new DeviceRegistrationResponse
            {
                RegistrationCode = result.RegistrationCode
            });
        }
    }
}