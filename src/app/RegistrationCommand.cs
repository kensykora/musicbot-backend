using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicBot.App
{
    public class RegistrationCommand
    {
        public RegistrationCommand(string registrationCode)
        {
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }
    }
}