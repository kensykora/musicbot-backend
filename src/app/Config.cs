using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicBot.App
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = new Config());

        public string Version => System.Environment.GetEnvironmentVariable("MusicBotVersion");
    }
}