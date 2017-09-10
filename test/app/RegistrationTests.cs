using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App;

using Xunit;

namespace MusicBot.App.Test
{
    public class RegistrationTests
    {
        private RegistrationTestsContext _ctx = new RegistrationTestsContext();

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
            var registerDeviceCommand = _ctx.GetStandardRegisterDeviceCommand(deviceIdIs: deviceId);

            // assert
            Assert.Equal(deviceId, registerDeviceCommand.DeviceId);
        }

        [Fact]
        public async Task RegisteringDevice_ReturnsRegistrationCode()
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
    }

    public class RegistrationTestsContext
    {
        public Guid StandardDeviceId = new Guid("0aecbea0-79ee-46e9-b1cc-a08737d1d01e");

        public RegisterDeviceCommand GetStandardRegisterDeviceCommand(Guid? deviceIdIs = null,
            bool useStandardValues = true)
        {
            if (useStandardValues && !deviceIdIs.HasValue)
            {
                deviceIdIs = StandardDeviceId;
            }

            Debug.Assert(deviceIdIs != null, nameof(deviceIdIs) + " != null");
            return new RegisterDeviceCommand(deviceIdIs.Value);
        }
    }
}