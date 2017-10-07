using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicBot.Functions
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = new Config());
        public string BetaKey => GetValue();
        public string SlackVerificationToken => GetValue();
        public string Version => GetValue();

        private string GetValue([CallerMemberName] string key = "")
        {
            var envVal = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(envVal))
            {
                return envVal;
            }

            return ConfigurationManager.AppSettings[key];
        }

    }
}
