using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MusicBot.App
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = new Config());

        public string Version => GetValue("MusicBotVersion");
        public string DocumentDbKey => GetValue("DocumentDbKey");
        public string DocumentDbServer => GetValue("DocumentDbServer");
        public string DocumentDbDatabaseId => GetValue("DocumentDbDatabaseId");
        public string IoTHubConnectionString => GetValue("IoTHubConnectionString");

        private string GetValue(string key)
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