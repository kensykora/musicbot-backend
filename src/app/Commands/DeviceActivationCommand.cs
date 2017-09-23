using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App.Data;

namespace MusicBot.App.Commands
{
    public class DeviceActivationCommand : CommandBase<VoidCommandResult>
    {
        private readonly IDocumentDbRepository<DeviceRegistration> _database;

        public DeviceActivationCommand(string registrationCode, IDocumentDbRepository<DeviceRegistration> database)
        {
            _database = database;
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }

        public override async Task<VoidCommandResult> ExecuteAsync()
        {
            var device =  await _database.FirstOrDefault(x => x.RegistrationCode.Equals(RegistrationCode));

            device.ActivatedOn = DateTime.UtcNow;

            return new VoidCommandResult();
        }
    }
}