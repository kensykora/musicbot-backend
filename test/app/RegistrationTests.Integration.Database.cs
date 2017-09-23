using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace MusicBot.App.Test
{
    [Trait("Category", "Integration")]
    public class RegistrationDatabaseIntegrationTests : IClassFixture<RegistrationTestsIntegrationContext>
    {
        private readonly RegistrationTestsIntegrationContext _ctx;

        public RegistrationDatabaseIntegrationTests(RegistrationTestsIntegrationContext ctx)
        {
            _ctx = ctx;
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000008d1d01e", "d1d01e")]
        [InlineData("00000000-0000-0000-0000-000008d1d01e", "d1d01e")]
        public async Task RegisterDevices_GeneratesSameCodes_ForSameDevices(string deviceId, string expectedCode)
        {
            // arrange
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(new Guid(deviceId));

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal(expectedCode, result.RegistrationCode);
        }

        [Theory]
        [InlineData("766cdbbf-d9fa-45d6-991c-f442a7e1d4ac")]
        [InlineData("2e1c4282-bfb4-471d-bda9-d3a28fd578f1")]
        [InlineData("6e938484-2efc-435c-92b3-2d6c798961a7")]
        [InlineData("a7a087da-50ed-4709-90d6-344e1b7be7b1")]
        [InlineData("bbb210b5-4b2e-4d22-9738-b5431215618f")]
        [InlineData("aef0072e-94f6-4d15-9667-19309d0f8fa9")]
        [InlineData("6d4647ae-a146-4dd2-917c-a55151ef02d6")]
        [InlineData("1490c4f2-4a0b-42ed-8bc5-7b9b71327b39")]
        [InlineData("060c3478-f7c4-41f0-a9fc-18aa7e09afd8")]
        [InlineData("9e3a76d1-59f2-4978-b023-49f8924ef2a8")]
        public async Task RegisterDevices_GeneratesPredictableCodes_ForDifferentDevices(string deviceId)
        {
            // arrange
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(new Guid(deviceId));

            // act
            var result = await registerDeviceCommand.ExecuteAsync();

            // assert
            Assert.Equal(deviceId.Substring(deviceId.Length - 6, 6), result.RegistrationCode);
        }
    }
}