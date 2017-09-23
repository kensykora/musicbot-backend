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
    public class ActivationTests : IClassFixture<ActivationTestsContext>
    {
        private readonly ActivationTestsContext _ctx;

        public ActivationTests(ActivationTestsContext ctx)
        {
            _ctx = ctx;
        }

        [Fact]
        public async Task ActivatingDevice_ForExistingDevice_ThatIsNotActivated_IsSuccessful()
        {
            // arrange
            var database = _ctx.GetStandardMockDatabase<DeviceRegistration>();
            database.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<DeviceRegistration, bool>>>()))
                .Returns(Task.FromResult(_ctx.StandardDeviceRegistration));
            var command = _ctx.GetStandardActivationCommand(databaseIs: database.Object);

            // act
            var response = await command.ExecuteAsync();

            // assert
            Assert.Equal(ActivationStatus.Success, response.Status);
        }

        [Fact]
        public async Task ActivatingDevice_ForNonExistingDevice_IsNotFound()
        {
            // arrange
            var command = _ctx.GetStandardActivationCommand();

            // act / assert
            var result = await command.ExecuteAsync();

            // assert
            Assert.Equal(ActivationStatus.NotFound, result.Status);

        }

        [Fact]
        public async Task ActivatingDevice_ThatIsAlreadyActivated_IsNotFound()
        {
            // arrange
            var registrationRecord = _ctx.StandardDeviceRegistration;
            registrationRecord.ActivatedOn = DateTime.UtcNow;

            var database = _ctx.GetStandardMockDatabase<DeviceRegistration>();
            database.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<DeviceRegistration, bool>>>()))
                .Returns(Task.FromResult(registrationRecord));
            var command = _ctx.GetStandardActivationCommand(databaseIs: database.Object);

            // act
            var response = await command.ExecuteAsync();

            // assert
            Assert.Equal(ActivationStatus.NotFound, response.Status);

        }
    }
}