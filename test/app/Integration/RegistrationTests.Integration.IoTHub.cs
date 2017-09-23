using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App.Test.Context;

using Xunit;

namespace MusicBot.App.Test.Integration
{
    [Trait("Category", "Integration")]
    public class RegistrationIoTHubIntegrationTests : IClassFixture<RegistrationTestsIoTHubIntegrationContext>
    {
        public RegistrationIoTHubIntegrationTests(RegistrationTestsIoTHubIntegrationContext ctx)
        {
            _ctx = ctx;
        }

        private readonly RegistrationTestsIoTHubIntegrationContext _ctx;

        [Fact]
        public async Task RegisterDevices_CanRegisterHubDevices()
        {
            // arrange
            var id = Guid.NewGuid();
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(id);

            // act
            await registerDeviceCommand.ExecuteAsync();

            // assert
            var device = await _ctx.IoTHubClient.GetByIdAsync(id);
            Assert.Equal(id.ToString(), device.Id);
        }

        [Fact]
        public async Task RegisterDevices_WorksWithMultipleCallsOfSameDevice()
        {
            // arrange
            var id = Guid.NewGuid();
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(id);

            // act
            var response1 = await registerDeviceCommand.ExecuteAsync();
            var response2 = await registerDeviceCommand.ExecuteAsync();

            // assert
            var device = await _ctx.IoTHubClient.GetByIdAsync(id);
            Assert.Equal(response1.RegistrationCode, response2.RegistrationCode);
        }
    }
}