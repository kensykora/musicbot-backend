using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicBot.Functions.Models
{
    public class SlackSlashCommandResponse
    {
        [JsonProperty("response_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageResponseType ResponseType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public enum MessageResponseType
    {
        [EnumMember(Value = "in_channel")] InChannel,
        [EnumMember(Value = "ephemeral")] Ephemeral
    }
}