using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;

using MusicBot.App.Commands;
using MusicBot.App.Data;
using MusicBot.App.Devices;

namespace MusicBot.App.Test
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
        // See https://github.com/Azure/azure-documentdb-dotnet/blob/master/docs/documentdb-nosql-local-emulator.md

        public DocumentDbRepository<DeviceRegistration> DeviceRegistrationDb => ConnectionFactory.Instance.DeviceRegistration;

        public RegistrationTestsDatabaseIntegrationContext()
        {
            ConnectionFactory.Instance.DeviceRegistration.ClearAllAsync().Wait();
        }

        public override RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null, IDeviceHub deviceHubIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && databaseIs == null)
                databaseIs = DeviceRegistrationDb;


            return base.GetStandardRegisterDeviceCommand(deviceIdIs, databaseIs, deviceHubIs, useStandardValues);
        }
    }

    public class RegistrationTestsContext
    {
        public Guid StandardDeviceId = new Guid("0aecbea0-79ee-46e9-b1cc-a08737d1d01e");
        public string StandardDeviceRegistrationCode = "d1d01e";

        public DeviceRegistration StandardDeviceRegistration => new DeviceRegistration
        {
            RegistrationCode = StandardDeviceRegistrationCode,
            DeviceId = StandardDeviceId
        };

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

        public Mock<IDocumentDbRepository<TDataType>> GetStandardMockDatabase<TDataType>() where TDataType : class
        {
            var result = new Mock<IDocumentDbRepository<TDataType>>();

            result.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TDataType, bool>>>()))
                .Returns(Task.FromResult(false));

            return result;
        }

        public Mock<IDeviceHub> GetStandardMockDeviceHub()
        {
            var result = new Mock<IDeviceHub>();

            return result;
        }
    }
}