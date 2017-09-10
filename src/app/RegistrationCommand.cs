using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App
{
    public class RegistrationCommand : CommandBase<VoidCommandResult>
    {
        public RegistrationCommand(string registrationCode)
        {
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }

        public override async Task<VoidCommandResult> ExecuteAsync()
        {
            return new VoidCommandResult();
        }
    }
}