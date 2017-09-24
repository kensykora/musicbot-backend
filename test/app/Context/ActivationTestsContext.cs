using System;
using System.Collections.Generic;
using System.Linq;

using MusicBot.App.Commands;
using MusicBot.App.Data;

namespace MusicBot.App.Test.Context
{
    public class ActivationTestsDatabaseIntegrationContext : ActivationTestsContext
    {
        public ActivationTestsDatabaseIntegrationContext()
        {
            ConnectionFactory.Instance.DeviceRegistration.ClearAllAsync().Wait();
        }
        // See https://github.com/Azure/azure-documentdb-dotnet/blob/master/docs/documentdb-nosql-local-emulator.md

        public DocumentDbRepository<DeviceRegistration> DeviceRegistrationDb =>
            ConnectionFactory.Instance.DeviceRegistration;

        public override DeviceActivationCommand GetStandardActivationCommand(string registrationCodeIs = null,
            string teamIdIs = null, string teamDomainIs = null, string channelIdIs = null, string channelNameIs = null,
            string userIdIs = null, string userNameIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null,
            bool useDefaults = true)
        {
            if (useDefaults && databaseIs == null)
                databaseIs = DeviceRegistrationDb;

            return base.GetStandardActivationCommand(registrationCodeIs, teamIdIs, teamDomainIs, channelIdIs, channelNameIs,
                userIdIs, userNameIs, databaseIs, useDefaults);
        }
    }

    public class ActivationTestsContext : BaseTestsContext
    {
        public string StandardUserName => "Steve";

        public string StandardUserId => "U2147483697";

        public string StandardChannelName => "test";

        public string StandardChannelId => "C2147483705";

        public string StandardTeamDomain => "example";

        public string StandardTeamId => "T0001";

        public virtual DeviceActivationCommand GetStandardActivationCommand(string registrationCodeIs = null,
            string teamIdIs = null, string teamDomainIs = null, string channelIdIs = null, string channelNameIs = null,
            string userIdIs = null, string userNameIs = null,
            IDocumentDbRepository<DeviceRegistration> databaseIs = null,
            bool useDefaults = true)
        {
            if (useDefaults && registrationCodeIs == null)
                registrationCodeIs = StandardDeviceRegistrationCode;

            if (useDefaults && teamIdIs == null)
                teamIdIs = StandardTeamId;

            if (useDefaults && teamDomainIs == null)
                teamDomainIs = StandardTeamDomain;

            if (useDefaults && channelIdIs == null)
                channelIdIs = StandardChannelId;

            if (useDefaults && channelNameIs == null)
                channelNameIs = StandardChannelName;

            if (useDefaults && userIdIs == null)
                userIdIs = StandardUserId;

            if (useDefaults && userNameIs == null)
                userNameIs = StandardUserName;

            if (useDefaults && databaseIs == null)
                databaseIs = GetStandardMockDatabase<DeviceRegistration>().Object;

            return new DeviceActivationCommand(registrationCodeIs, teamIdIs, teamDomainIs, channelIdIs, channelNameIs,
                userIdIs, userNameIs, databaseIs);
        }
    }
}