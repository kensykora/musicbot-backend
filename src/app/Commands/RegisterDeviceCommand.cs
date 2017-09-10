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
        public const int CodeLength = 6;
        private readonly IDocumentDbRepository<DeviceRegistration> _database;

        public RegisterDeviceCommand(Guid deviceId, IDocumentDbRepository<DeviceRegistration> database)
        {
            _database = database;
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }

        public override async Task<RegisterDeviceCommandResult> ExecuteAsync()
        {
            var code = await GenerateCode(DeviceId);
            await _database.AddOrUpdateAsync(new DeviceRegistration
            {
                DeviceId = DeviceId,
                RegistrationCode = code
            });

            return new RegisterDeviceCommandResult(code);
        }

        private async Task<string> GenerateCode(Guid deviceId)
        {
            var code = deviceId.ToString().Replace("-","");
            var i = code.Length - CodeLength;

            while (i >= 0)
            {
                var test = code.Substring(i, CodeLength);
                if (await _database.CountAsync(x => x.RegistrationCode == test) == 0)
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