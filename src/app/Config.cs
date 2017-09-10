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

        public string Version => System.Environment.GetEnvironmentVariable("MusicBotVersion");
        public string DocumentDbKey => ConfigurationManager.AppSettings["DocumentDbKey"];
        public string DocumentDbServer => ConfigurationManager.AppSettings["DocumentDbServer"];
        public string DocumentDbDatabaseId => ConfigurationManager.AppSettings["DocumentDbDatabaseId"];
    }
}