using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MusicBot.App.Data;

namespace MusicBot.App.Commands
{
    public class DeviceActivationCommand : CommandBase<DeviceActivationCommandResponse>
    {
        private readonly IDocumentDbRepository<DeviceRegistration> _database;

        public DeviceActivationCommand(string registrationCode, string teamId, string teamDomain, string channelId, string channelName, string userId, string userName, IDocumentDbRepository<DeviceRegistration> database)
        {
            TeamId = teamId;
            TeamDomain = teamDomain;
            ChannelId = channelId;
            ChannelName = channelName;
            UserId = userId;
            UserName = userName;
            _database = database;
            RegistrationCode = registrationCode;
        }

        public string RegistrationCode { get; }

        public string TeamId { get; }

        public string TeamDomain { get; }
        public string ChannelId { get; }
        public string ChannelName { get; }
        public string UserId { get; }
        public string UserName { get; }

        public override async Task<DeviceActivationCommandResponse> ExecuteAsync()
        {
            var device =  await _database.FirstOrDefault(x => x.RegistrationCode.Equals(RegistrationCode.ToUpper()));

            if (device == null)
            {
                return new DeviceActivationCommandResponse(ActivationStatus.NotFound);
            }

            if (device.ActivatedOn.HasValue)
            {
                return new DeviceActivationCommandResponse(ActivationStatus.NotFound);
            }

            device.ActivatedOn = DateTime.UtcNow;
            device.TeamId = TeamId;
            device.TeamDomain = TeamDomain;
            device.ChannelId = ChannelId;
            device.ChannelName = ChannelName;
            device.UserName = UserName;
            device.UserId = UserId;

            await _database.UpdateItemAsync(device);

            return new DeviceActivationCommandResponse(ActivationStatus.Success);
        }
    }

    public class DeviceActivationCommandResponse
    {
        public DeviceActivationCommandResponse(ActivationStatus status)
        {
            Status = status;
        }

        public ActivationStatus Status { get; }
    }

    public enum ActivationStatus
    {
        Success,
        NotFound
    }
}