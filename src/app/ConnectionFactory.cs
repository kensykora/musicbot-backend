using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;

using MusicBot.App.Data;
using MusicBot.App.Devices;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicBot.App
{
    public class ConnectionFactory
    {
        private static ConnectionFactory _instance;

        public DocumentClient DatabaseClient { get; }
        public IoTHub IoTHubClient { get; }


        private ConnectionFactory()
        {
            IoTHubClient = new IoTHub(Config.Instance.IoTHubConnectionString);

            DatabaseClient =
                new DocumentClient(new Uri(Config.Instance.DocumentDbServer), Config.Instance.DocumentDbKey);

            CreateDatabaseIfNotExistsAsync().Wait();

            DeviceRegistration = new DocumentDbRepository<DeviceRegistration>(DatabaseClient,
                Config.Instance.DocumentDbDatabaseId, typeof(DeviceRegistration).Name);
        }

        public DocumentDbRepository<DeviceRegistration> DeviceRegistration { get; }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await DatabaseClient.ReadDatabaseAsync(
                    UriFactory.CreateDatabaseUri(Config.Instance.DocumentDbDatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await DatabaseClient.CreateDatabaseAsync(new Database {Id = Config.Instance.DocumentDbDatabaseId});
                }
                else
                {
                    throw;
                }
            }
        }

        public static ConnectionFactory Instance => _instance ?? (_instance = new ConnectionFactory());
    }
}