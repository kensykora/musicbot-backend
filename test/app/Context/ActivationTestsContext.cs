using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MusicBot.App.Commands;
using MusicBot.App.Data;

namespace MusicBot.App.Test.Context
{
    public class ActivationTestsContext : BaseTestsContext
    {
        public DeviceActivationCommand GetStandardActivationCommand(string registrationCodeIs = null, IDocumentDbRepository<DeviceRegistration> databaseIs = null, bool useDefaults = true)
        {
            if (useDefaults && registrationCodeIs == null)
            {
                registrationCodeIs = StandardDeviceRegistrationCode;
            }

            if (useDefaults && databaseIs == null)
            {
                databaseIs = GetStandardMockDatabase<DeviceRegistration>().Object;
            }

            return new DeviceActivationCommand(registrationCodeIs, databaseIs);
        }

        public string StandardDeviceRegistrationCode => "d1d01e";
    }
}
