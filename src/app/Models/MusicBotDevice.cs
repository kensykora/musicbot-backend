using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;

namespace MusicBot.App.Models
{
    public class MusicBotDevice
    {
        private readonly Device _device;
        public MusicBotDevice() { }
        public MusicBotDevice(Device device)
        {
            _device = device;
        }

        public string Id => _device.Id;
    }
}
