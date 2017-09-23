using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;

using MusicBot.App.Commands;
using MusicBot.App.Data;
using MusicBot.App.Test.Context;

using Xunit;

namespace MusicBot.App.Test
{
    public class RegistrationTests : IClassFixture<RegistrationTestsContext>
    {
        public RegistrationTests(RegistrationTestsContext ctx)
        {
            _ctx = ctx;
        }

        private readonly RegistrationTestsContext _ctx;

        [Fact]
        public void RegisterDevice_AcceptsGuid()
        {
            // arrange
            var deviceId = _ctx.StandardDeviceId;

            // act
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceId);

            // assert
            Assert.Equal(deviceId, registerDeviceCommand.DeviceId);
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
            Assert.Equal("d1d01e", result.RegistrationCode);
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
            database.Verify(x =>
                x.CreateItemAsync(It.Is<DeviceRegistration>(r => r.DeviceId == _ctx.StandardDeviceId)));
        }

        [Fact]
        public async Task RegisterDevice_RegistersWithIoTHub()
        {
            // arrange
            var deviceHub = _ctx.GetStandardMockDeviceHub();
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceHubIs: deviceHub.Object);

            // act
            await registerDeviceCommand.ExecuteAsync();

            // assert
            deviceHub.Verify(x => x.RegisterDeviceAsync(It.Is<Guid>(y => y.Equals(_ctx.StandardDeviceId))));
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
            var deviceId = _ctx.StandardDeviceId;
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
        public async Task RegisterDevice_ThrowsArgumentException_IfGuidIsEmpty()
        {
            // arrange
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(new Guid());

            // act / assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await registerDeviceCommand.ExecuteAsync());
        }

        [Fact]
        public async Task RegisterDevice_UsesAlternateCode_OnCollision()
        {
            // arrange
            var i = 0;
            var database = _ctx.GetStandardMockDatabase<DeviceRegistration>();
            database.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<DeviceRegistration, bool>>>())).Returns(() =>
            {
                i++;
                return Task.FromResult(i == 1);
            });
            var deviceId =
                new Guid(
                    "00000000-0000-0000-0000-000008d1d01e"); // Should collide on the first generation pass for "d1d01e", using "8d1d01" instead
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceId, database.Object);

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal("8d1d01", result.RegistrationCode);
        }

        [Fact]
        public async Task RegisterDevice_UsesExisstingResponse_ForSameDeviceId()
        {
            // arrange
            var database = _ctx.GetStandardMockDatabase<DeviceRegistration>();
            database.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<DeviceRegistration, bool>>>())).Returns(() =>
                Task.FromResult(_ctx.StandardDeviceRegistration));
            var registerDeviceCommand =
                _ctx.GetStandardRegisterDeviceCommand(_ctx.StandardDeviceRegistration.DeviceId, database.Object);

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal(_ctx.StandardDeviceRegistration.RegistrationCode, result.RegistrationCode);
        }
    }
}