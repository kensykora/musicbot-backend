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

namespace MusicBot.Functions
{
    public static class Activate
    {
        [FunctionName("activate-device")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "device/activate")] HttpRequestMessage req,
            TraceWriter log)
        {
            var data = await req.Content.ReadAsFormDataAsync();
            var resp = new SlackSlashCommandRequest(data);

            if (resp.Token != Config.Instance.SlackVerificationToken)
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            if (resp.Text == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}