using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Common.Exceptions;

using MusicBot.App.Data;
using MusicBot.App.Devices;

namespace MusicBot.App.Commands
{
    public class RegisterDeviceCommand : CommandBase<RegisterDeviceCommand.RegisterDeviceCommandResult>
    {
        public const int CodeLength = 6;
        private readonly IDocumentDbRepository<DeviceRegistration> _database;
        private readonly IDeviceHub _deviceHub;

        public RegisterDeviceCommand(Guid deviceId, IDocumentDbRepository<DeviceRegistration> database, IDeviceHub deviceHub)
        {
            _database = database;
            _deviceHub = deviceHub;
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }

        public override async Task<RegisterDeviceCommandResult> ExecuteAsync()
        {
            if (DeviceId == Guid.Empty)
            {
                throw new ArgumentException("DeviceId must be set");
            }

            var existing = await _database.FirstOrDefault(x => x.DeviceId == DeviceId);
            if (existing != null)
            {
                return new RegisterDeviceCommandResult(existing.RegistrationCode);
            }

            var code = await GenerateCode(DeviceId);
            await _database.CreateItemAsync(new DeviceRegistration
            {
                DeviceId = DeviceId,
                RegistrationCode = code
            });

            try
            {
                await _deviceHub.RegisterDeviceAsync(DeviceId);
            }
            catch (DeviceAlreadyExistsException)
            {
                // TODO: Log notice of subsequent registration
            }

            return new RegisterDeviceCommandResult(code);
        }

        private async Task<string> GenerateCode(Guid deviceId)
        {
            var code = deviceId.ToString().Replace("-","");
            var i = code.Length - CodeLength;

            while (i >= 0)
            {
                var test = code.Substring(i, CodeLength).ToUpper();
                if (!await _database.AnyAsync(x => x.RegistrationCode.Equals(test)))
                {
                    return test;
                }

                i--;
            }

            throw new ApplicationException("Unable to generate a unique code.");
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