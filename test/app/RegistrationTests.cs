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
        public void RegisterDevice_AcceptsRegistrationCode()
        {
            // arrange / act
            var registrationCommand = new RegistrationCommand("ABC123");

            // assert
            Assert.Equal("ABC123", registrationCommand.RegistrationCode);
        }
    }
}