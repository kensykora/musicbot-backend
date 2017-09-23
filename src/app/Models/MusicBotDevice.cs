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
        public MusicBotDevice() { }
        public MusicBotDevice(Device device)
        {
            DeviceId = new Guid(device.Id);
        }

        public Guid DeviceId { get; set; }
    }
}
