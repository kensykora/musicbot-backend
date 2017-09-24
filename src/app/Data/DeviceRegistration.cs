using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicBot.App.Data
{
    public class DeviceRegistration : CosmosDocument
    {
        public string RegistrationCode { get; set; }

        public DateTime? ActivatedOn { get; set; }
        public string ChannelName { get; set; }
        public string ChannelId { get; set; }
        public string TeamDomain { get; set; }
        public string TeamId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CosmosDocument
    {
        // ReSharper disable once InconsistentNaming
        public Guid id { get; set; }
    }
}