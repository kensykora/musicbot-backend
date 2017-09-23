using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MusicBot.App.Models;

namespace MusicBot.App.Devices
{
    public interface IDeviceHub
    {
        Task RegisterDeviceAsync(Guid deviceId);

        Task<Models.MusicBotDevice> GetByIdAsync(Guid id);
    }
}
