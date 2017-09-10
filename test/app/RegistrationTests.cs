using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using DocumentDB.Repository;

using Microsoft.Azure.Documents.Client;

using Moq;

using MusicBot.App.Commands;
using MusicBot.App.Data;

using Xunit;

namespace MusicBot.App.Test
{
    public class RegistrationTests
    {
        private readonly RegistrationTestsContext _ctx = new RegistrationTestsContext();

        [Fact]
        public void Register_AcceptsRegistrationCode()
        {
            // arrange
            var command = "ABC123";

            // act
            var registerCommand = new RegistrationCommand(command);

            // assert
            Assert.Equal(command, registerCommand.RegistrationCode);
        }

        [Fact]
        public void RegisterDevice_AcceptsGuid()
        {
            // arrange
            var deviceId = Guid.NewGuid();

            // act
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceId);

            // assert
            Assert.Equal(deviceId, registerDeviceCommand.DeviceId);
        }

        [Fact]
        public async Task RegisterDevice_PersistsGuidToStorage()
        {
            // arrange
            var database = _ctx.GetStandardMockDatabase<DeviceRegistration>();
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(databaseIs: database.Object);

            // act
            await registerDeviceCommand.ExecuteAsync();

            // assert
            database.Verify(x => x.AddOrUpdateAsync(It.Is<DeviceRegistration>(r => r.DeviceId == _ctx.StandardDeviceId),
                It.IsAny<RequestOptions>()));
        }

        [Fact]
        public async Task RegisterDevice_ReturnsRegistrationCode()
        {
            // arrange
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand();

            // act
            var result = await registerDeviceCommand.ExecuteAsync();


            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.RegistrationCode);
            Assert.NotEmpty(result.RegistrationCode);
        }

        [Fact]
        public async Task RegisterDevice_ReturnsSameCode_ForSameDevices()
        {
            // arrange
            var deviceId = Guid.NewGuid();
            var registerDeviceCommand1 = _ctx.GetStandardRegisterDeviceCommand(deviceId);
            var registerDeviceCommand2 = _ctx.GetStandardRegisterDeviceCommand(deviceId);

            // act
            var result1 = await registerDeviceCommand1.ExecuteAsync();
            var result2 = await registerDeviceCommand2.ExecuteAsync();


            // assert
            Assert.NotNull(result1);
            Assert.NotNull(result1.RegistrationCode);
            Assert.NotEmpty(result1.RegistrationCode);
            Assert.Equal(result1.RegistrationCode, result2.RegistrationCode);
        }

        [Fact]
        public async Task RegisterDevice_GeneratesProperCode()
        {
            // arrange
            var deviceId = new Guid("0aecbea0-79ee-46e9-b1cc-a08737d1d01e");
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceId);

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal("D1D01E", result.RegistrationCode, StringComparer.OrdinalIgnoreCase);
        }
    }

    public class RegistrationTestsContext
    {
        public Guid StandardDeviceId = new Guid("0aecbea0-79ee-46e9-b1cc-a08737d1d01e");

        public RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && !deviceIdIs.HasValue)
                deviceIdIs = StandardDeviceId;

            if (useStandardValues && databaseIs == null)
            {
                databaseIs = GetStandardMockDatabase<DeviceRegistration>().Object;
            }

            Debug.Assert(deviceIdIs != null, nameof(deviceIdIs) + " != null");

            return new RegisterDeviceCommand(deviceIdIs.Value, databaseIs);
        }

        public Mock<IDocumentDbRepository<TDataType>> GetStandardMockDatabase<TDataType>() where TDataType : class
        {
            return new Mock<IDocumentDbRepository<TDataType>>();
        }
    }
}