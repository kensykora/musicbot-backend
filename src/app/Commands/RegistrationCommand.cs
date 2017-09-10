using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App.Commands
{
    public class RegistrationCommand : CommandBase<VoidCommandResult>
    {
        public RegistrationCommand(string registrationCode)
        {
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }

        public override Task<VoidCommandResult> ExecuteAsync()
        {
            return Task.FromResult(new VoidCommandResult());
        }
    }
}