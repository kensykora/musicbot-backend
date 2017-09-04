using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace MusicBot.Functions
{
    public static class Play
    {
        [FunctionName("play")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Play Request");

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}