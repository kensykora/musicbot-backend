using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App.Data;

namespace MusicBot.App.Commands
{
    public class DeviceActivationCommand : CommandBase<DeviceActivationCommandResponse>
    {
        private readonly IDocumentDbRepository<DeviceRegistration> _database;

        public DeviceActivationCommand(string registrationCode, IDocumentDbRepository<DeviceRegistration> database)
        {
            _database = database;
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }

        public override async Task<DeviceActivationCommandResponse> ExecuteAsync()
        {
            var device =  await _database.FirstOrDefault(x => x.RegistrationCode.Equals(RegistrationCode.ToUpper()));

            if (device == null)
            {
                return new DeviceActivationCommandResponse(ActivationStatus.NotFound);
            }

            if (device.ActivatedOn.HasValue)
            {
                return new DeviceActivationCommandResponse(ActivationStatus.NotFound);
            }

            device.ActivatedOn = DateTime.UtcNow;

            return new DeviceActivationCommandResponse(ActivationStatus.Success);
        }
    }

    public class DeviceActivationCommandResponse
    {
        public DeviceActivationCommandResponse(ActivationStatus status)
        {
            Status = status;
        }

        public ActivationStatus Status { get; }
    }

    public enum ActivationStatus
    {
        Success,
        NotFound
    }
}