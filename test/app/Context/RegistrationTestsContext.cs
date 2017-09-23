using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using MusicBot.App.Commands;
using MusicBot.App.Data;
using MusicBot.App.Devices;

namespace MusicBot.App.Test.Context
{
    public class RegistrationTestsIoTHubIntegrationContext : RegistrationTestsContext
    {
        public IoTHub IoTHubClient => ConnectionFactory.Instance.IoTHubClient;

        public override RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null, IDeviceHub deviceHubIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && deviceHubIs == null)
                deviceHubIs = IoTHubClient;


            return base.GetStandardRegisterDeviceCommand(deviceIdIs, databaseIs, deviceHubIs, useStandardValues);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global - Used by XUnit
    public class RegistrationTestsDatabaseIntegrationContext : RegistrationTestsContext
    {
        public RegistrationTestsDatabaseIntegrationContext()
        {
            ConnectionFactory.Instance.DeviceRegistration.ClearAllAsync().Wait();
        }
        // See https://github.com/Azure/azure-documentdb-dotnet/blob/master/docs/documentdb-nosql-local-emulator.md

        public DocumentDbRepository<DeviceRegistration> DeviceRegistrationDb =>
            ConnectionFactory.Instance.DeviceRegistration;

        public override RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null, IDeviceHub deviceHubIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && databaseIs == null)
                databaseIs = DeviceRegistrationDb;


            return base.GetStandardRegisterDeviceCommand(deviceIdIs, databaseIs, deviceHubIs, useStandardValues);
        }
    }

    public class RegistrationTestsContext : BaseTestsContext
    {
        public virtual RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null, IDeviceHub deviceHubIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && !deviceIdIs.HasValue)
                deviceIdIs = StandardDeviceId;

            if (useStandardValues && databaseIs == null)
                databaseIs = GetStandardMockDatabase<DeviceRegistration>().Object;

            if (useStandardValues && deviceHubIs == null)
                deviceHubIs = GetStandardMockDeviceHub().Object;

            Debug.Assert(deviceIdIs != null, nameof(deviceIdIs) + " != null");

            return new RegisterDeviceCommand(deviceIdIs.Value, databaseIs, deviceHubIs);
        }
    }
}