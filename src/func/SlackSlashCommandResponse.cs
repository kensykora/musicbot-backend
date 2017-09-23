using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MusicBot.Functions
{
    public class SlackSlashCommandResponse
    {
        [JsonProperty("response_type")]
        public MessageResponseType ResponseType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public enum MessageResponseType
    {
        [EnumMember(Value = "in_channel")]
        InChannel,
        [EnumMember(Value = "ephemeral")]
        Ephemeral
    }
}
