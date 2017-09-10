using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App
{
    public class RegisterDeviceCommand : CommandBase<RegisterDeviceCommand.RegisterDeviceCommandResult>
    {
        public RegisterDeviceCommand(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }

        public override Task<RegisterDeviceCommandResult> ExecuteAsync()
        {
            return Task.FromResult(new RegisterDeviceCommandResult()
            {
                RegistrationCode = "ABC123"
            });
        }

        public class RegisterDeviceCommandResult
        {
            public string RegistrationCode { get; set; }
        }
    }
}