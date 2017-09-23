using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;

using Newtonsoft.Json;

namespace MusicBot.App.Data
{
    public class DeviceRegistration
    {
        public Guid DeviceId { get; set; }

        public string RegistrationCode { get; set; }

        public DateTime? ActivatedOn { get; set; }
    }
}
