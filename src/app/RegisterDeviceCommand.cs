using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBot.App
{
    public class RegisterDeviceCommand
    {
        public Guid DeviceId { get; }

        public RegisterDeviceCommand(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
