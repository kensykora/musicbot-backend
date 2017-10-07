using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using MusicBot.Functions.Models;

using Newtonsoft.Json;

namespace MusicBot.Functions.Test.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ReserializeContent<T>(this HttpResponseMessage response) where T : class
        {
            if (response.Content == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
