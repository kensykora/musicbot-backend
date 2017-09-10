using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App;

using Xunit;

namespace MusicBot.App.Test
{
    public class RegistrationTests
    {
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
            var registerDeviceCommand = new RegisterDeviceCommand(deviceId);

            // assert
            Assert.Equal(deviceId, registerDeviceCommand.DeviceId);
        }
    }
}