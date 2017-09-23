using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;

using MusicBot.App.Commands;
using MusicBot.App.Data;

namespace MusicBot.App.Test
{
    // ReSharper disable once ClassNeverInstantiated.Global - Used by XUnit
    public class RegistrationTestsIntegrationContext : RegistrationTestsContext
    {
        // See https://github.com/Azure/azure-documentdb-dotnet/blob/master/docs/documentdb-nosql-local-emulator.md

        public RegistrationTestsIntegrationContext()
        {
            ConnectionFactory.Instance.DeviceRegistration.ClearAllAsync().Wait();
        }

        public override RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && databaseIs == null)
                databaseIs = ConnectionFactory.Instance.DeviceRegistration;


            return base.GetStandardRegisterDeviceCommand(deviceIdIs, databaseIs, useStandardValues);
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
            IDocumentDbRepository<DeviceRegistration> databaseIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && !deviceIdIs.HasValue)
                deviceIdIs = StandardDeviceId;

            if (useStandardValues && databaseIs == null)
                databaseIs = GetStandardMockDatabase<DeviceRegistration>().Object;

            Debug.Assert(deviceIdIs != null, nameof(deviceIdIs) + " != null");

            return new RegisterDeviceCommand(deviceIdIs.Value, databaseIs);
        }

        public Mock<IDocumentDbRepository<TDataType>> GetStandardMockDatabase<TDataType>() where TDataType : class
        {
            var result = new Mock<IDocumentDbRepository<TDataType>>();

            result.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TDataType, bool>>>()))
                .Returns(Task.FromResult(false));

            return result;
        }
    }
}