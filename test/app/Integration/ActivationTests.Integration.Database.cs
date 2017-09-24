using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MusicBot.App.Data;
using MusicBot.App.Test.Context;

using Xunit;

namespace MusicBot.App.Test.Integration
{
    [Trait("Category", "Integration")]
    [Trait("IntegrationType", "Database")]
    public class ActivationTests : IClassFixture<ActivationTestsDatabaseIntegrationContext>,
        IClassFixture<RegistrationTestsContext>
    {
        private readonly ActivationTestsDatabaseIntegrationContext _ctx;
        private readonly RegistrationTestsContext _regCtx;

        public ActivationTests(ActivationTestsDatabaseIntegrationContext ctx, RegistrationTestsContext regCtx)
        {
            _ctx = ctx;
            _regCtx = regCtx;
        }

        [Fact]
        public async Task ActivatingADevice_StoresSlackValues()
        {
            // arrange
            await _ctx.DeviceRegistrationDb.CreateItemAsync(_regCtx.StandardDeviceRegistration);
            var command = _ctx.GetStandardActivationCommand();

            // act
            await command.ExecuteAsync();
            var record = await _ctx.DeviceRegistrationDb.FirstOrDefault(x => x.id == _regCtx.StandardDeviceId);

            // assert
            Assert.Equal(command.ChannelId, record.ChannelId);
            Assert.Equal(command.ChannelName, record.ChannelName);
            Assert.Equal(command.TeamDomain, record.TeamDomain);
            Assert.Equal(command.TeamId, record.TeamId);
            Assert.Equal(command.UserId, record.UserId);
            Assert.Equal(command.UserName, record.UserName);
        }
    }
}