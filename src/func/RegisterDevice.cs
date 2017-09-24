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
            if (req.Headers.Authorization == null
                || !req.Headers.Authorization.Scheme.Equals("BetaKey", StringComparison.OrdinalIgnoreCase) 
                || !req.Headers.Authorization.Parameter.Equals(Config.Instance.BetaKey))
            {
                return req.CreateResponse(HttpStatusCode.Unauthorized, "BetaKey authorization required");
            }

            var body = await req.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(body))
                return req.CreateResponse(HttpStatusCode.BadRequest, "Id is required");

            HttpResponseMessage errorResponse = null;
            var registration =
                JsonConvert.DeserializeObject<DeviceRegistrationRequest>(await req.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    Error = (obj, args) =>
                    {
                        errorResponse = req.CreateResponse(HttpStatusCode.BadRequest, args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    }
                });

            if (errorResponse != null)
            {
                return errorResponse;
            }

            if (registration?.DeviceId == null || registration?.DeviceId == Guid.Empty)
            {
                log.Error("Invalid Request - Id is required.");
                return req.CreateResponse(HttpStatusCode.BadRequest, "Id is required");
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