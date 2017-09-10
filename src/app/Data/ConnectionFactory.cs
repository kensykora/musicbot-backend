using System;
using System.Collections.Generic;
using System.Linq;

using DocumentDB.Repository;

using Microsoft.Azure.Documents.Client.TransientFaultHandling;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicBot.App.Data
{
    public class ConnectionFactory
    {
        private static ConnectionFactory _instance;
        private readonly IReliableReadWriteDocumentClient _client;

        private ConnectionFactory()
        {
            _client = new DocumentDbInitializer().GetClient(Config.Instance.DocumentDbServer,
                Config.Instance.DocumentDbKey);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        public static ConnectionFactory Instance => _instance ?? (_instance = new ConnectionFactory());

        public IDocumentDbRepository<DeviceRegistration> DeviceRegistration =>
            new DocumentDbRepository<DeviceRegistration>(_client, Config.Instance.DocumentDbDatabaseId);
    }
}