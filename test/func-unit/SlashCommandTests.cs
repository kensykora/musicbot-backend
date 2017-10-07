using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Host;

using MusicBot.Functions.Models;
using MusicBot.Functions.Test.Context;
using MusicBot.Functions.Test.Extensions;

using Xunit;

namespace MusicBot.Functions.Test
{
    public class SlashCommandTests : IClassFixture<SlashCommandTestsContext>
    {
        private readonly SlashCommandTestsContext _ctx;

        public SlashCommandTests(SlashCommandTestsContext ctx)
        {
            _ctx = ctx;
        }

        [Fact]
        public async Task RunFunction_CreatesEphemeralResponse_WithInvalidCommand()
        {
            //arrange
            var req = _ctx.GetStandardSlackHttpRequestMessage();

            // act
            var response = await SlashCommand.Run(req, new TraceMonitor());
            var data = await response.ReserializeContent<SlackSlashCommandResponse>();

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(MessageResponseType.Ephemeral, data.ResponseType);
        }
    }
}
