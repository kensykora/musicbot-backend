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

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public MessageResponseStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("sub_status")]
        public MessageResponseSubStatus? SubStatus { get; set; }
    }

    public enum MessageResponseType
    {
        [EnumMember(Value = "in_channel")] InChannel,
        [EnumMember(Value = "ephemeral")] Ephemeral
    }

    public enum MessageResponseStatus
    {
        [EnumMember(Value = "success")] Success,
        [EnumMember(Value = "failure")] Failure
    }

    public enum MessageResponseSubStatus
    {
        [EnumMember(Value = "internal")] Internal,
        [EnumMember(Value = "validation")] Validation,
        [EnumMember(Value = "not_found")] NotFound,
        [EnumMember(Value = "unknown_command")] UnknownCommand
    }
}