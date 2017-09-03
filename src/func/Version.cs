using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Reflection;

namespace MusicBot.Functions
{
    public static class Version
    {
        public static object AssemblyInfo { get; private set; }

        [FunctionName("Version")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK, Assembly.GetExecutingAssembly().FullName);
        }
    }
}
