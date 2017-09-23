using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBot.App.Devices
{
    public interface IDeviceHub
    {
        Task RegisterDeviceAsync(Guid deviceId);
    }
}
