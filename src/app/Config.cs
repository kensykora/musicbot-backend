using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MusicBot.App
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = new Config());

        public string Version => GetValue();
        public string DocumentDbKey => GetValue();
        public string DocumentDbServer => GetValue();
        public string DocumentDbDatabaseId => GetValue();
        public string IoTHubConnectionString => GetValue();
        public string BetaKey => GetValue();

        private string GetValue([CallerMemberName]string key = "")
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