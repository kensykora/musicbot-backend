using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;

using MusicBot.App.Models;

namespace MusicBot.App.Devices
{
    public class IoTHub : IDeviceHub
    {
        private readonly RegistryManager _registryManager;

        public IoTHub(string connectionString)
        {
            _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        }
        public async Task RegisterDeviceAsync(Guid deviceId)
        {
            if (deviceId == Guid.Empty)
            {
                throw new ArgumentException("deviceId must be a valid guid", nameof(deviceId));
            }
            await _registryManager.AddDeviceAsync(new Device(deviceId.ToString()));
        }

        public async Task<MusicBotDevice> GetByIdAsync(Guid id)
        {
            var device = await _registryManager.GetDeviceAsync(id.ToString());

            return new MusicBotDevice(device);
        }
    }
}
