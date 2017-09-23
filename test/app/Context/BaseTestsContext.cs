using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;

using MusicBot.App.Data;
using MusicBot.App.Devices;

namespace MusicBot.App.Test.Context
{
    public class BaseTestsContext
    {
        public Guid StandardDeviceId = new Guid("0aecbea0-79ee-46e9-b1cc-a08737d1d01e");
        public string StandardDeviceRegistrationCode = "d1d01e";

        public DeviceRegistration StandardDeviceRegistration => new DeviceRegistration
        {
            RegistrationCode = StandardDeviceRegistrationCode,
            DeviceId = StandardDeviceId
        };

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