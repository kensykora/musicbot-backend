using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

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
                    version = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(Config)).Location)
                        .FileVersion
                });
        }
    }
}