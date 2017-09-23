using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App.Test.Context;

using Xunit;

namespace MusicBot.App.Test.Integration
{
    [Trait("Category", "Integration")]
    [Trait("IntegrationType", "Database")]
    public class RegistrationDatabaseIntegrationTests : IClassFixture<RegistrationTestsDatabaseIntegrationContext>
    {
        private readonly RegistrationTestsDatabaseIntegrationContext _ctx;

        public RegistrationDatabaseIntegrationTests(RegistrationTestsDatabaseIntegrationContext ctx)
        {
            _ctx = ctx;
        }

        [Theory]
        [InlineData("ad2985f8-5c91-4016-8728-c0a55613401c")]
        [InlineData("60209b1b-f7fb-4ae5-ae1f-328a2ccda5dd")]
        [InlineData("107d0ae0-696b-41bd-9daf-78e617e846e9")]
        [InlineData("14f30a43-fc28-4d45-8f2e-1a1edfd35a6a")]
        [InlineData("f8ac1f08-304a-4855-9992-642c88dcd135")]
        public async Task RegisterDevices_CanInsertRecords(string deviceId)
        {
            // arrange
            var id = new Guid(deviceId);
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(id);

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            var record = _ctx.DeviceRegistrationDb.FirstOrDefault(x => x.DeviceId == id);
            Assert.Equal(result.RegistrationCode, record.Result.RegistrationCode);
        }
    }
}