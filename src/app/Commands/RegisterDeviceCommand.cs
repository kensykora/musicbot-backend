using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DocumentDB.Repository;

using MusicBot.App.Data;

namespace MusicBot.App.Commands
{
    public class RegisterDeviceCommand : CommandBase<RegisterDeviceCommand.RegisterDeviceCommandResult>
    {
        private readonly IDocumentDbRepository<DeviceRegistration> _database;

        public RegisterDeviceCommand(Guid deviceId, IDocumentDbRepository<DeviceRegistration> database)
        {
            _database = database;
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }

        public override async Task<RegisterDeviceCommandResult> ExecuteAsync()
        {
            var code = "ABC123";
            await _database.AddOrUpdateAsync(new DeviceRegistration()
            {
                DeviceId = DeviceId,
                RegistrationCode = code
            });

            return new RegisterDeviceCommandResult(code);
        }

        public class RegisterDeviceCommandResult
        {
            public RegisterDeviceCommandResult(string code)
            {
                RegistrationCode = code;
            }
            public string RegistrationCode { get; set; }
        }
    }
}