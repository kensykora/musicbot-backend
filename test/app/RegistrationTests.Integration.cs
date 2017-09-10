using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;

using MusicBot.App.Data;

using Xunit;

namespace MusicBot.App.Test
{
    public class RegistrationIntegrationTests: IClassFixture<RegistrationTestsIntegrationContext>
    {
        private readonly RegistrationTestsIntegrationContext _ctx;

        public RegistrationIntegrationTests(RegistrationTestsIntegrationContext ctx)
        {
            _ctx = ctx;
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000008d1d01e", "d1d01e")]
        [InlineData("00000000-0000-0000-0000-000008d1d01e", "d1d01e")]
        public async Task RegisterDevices_GeneratesSameCodes_ForSameDevices(string deviceId, string expectedCode)
        {
            // arrange
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceIdIs: new Guid(deviceId));

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal(expectedCode, result.RegistrationCode);

        }
    }
}