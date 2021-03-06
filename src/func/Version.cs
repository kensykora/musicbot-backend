using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using MusicBot.App;

namespace MusicBot.Functions
{
    public static class Version
    {
        public static object AssemblyInfo { get; private set; }

        [FunctionName("version")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req)
        {
            return req.CreateResponse(HttpStatusCode.OK,
                new
                {
                    version = Config.Instance.Version
                });
        }
    }
}