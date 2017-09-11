using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Client.TransientFaultHandling;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicBot.App.Data
{
    public class ConnectionFactory
    {
        private static ConnectionFactory _instance;
        public DocumentClient Client { get; }

        private ConnectionFactory()
        {
            Client = new DocumentClient(new Uri(Config.Instance.DocumentDbServer), Config.Instance.DocumentDbKey);

            CreateDatabaseIfNotExistsAsync().Wait();

            DeviceRegistration = new DocumentDbRepository<DeviceRegistration>(Client, Config.Instance.DocumentDbDatabaseId, typeof(DeviceRegistration).Name);
        }

        public DocumentDbRepository<DeviceRegistration> DeviceRegistration { get; }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(Config.Instance.DocumentDbDatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await Client.CreateDatabaseAsync(new Database { Id = Config.Instance.DocumentDbDatabaseId });
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