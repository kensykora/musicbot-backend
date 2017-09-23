# Musicbot-Backend

[![Build status](https://ci.appveyor.com/api/projects/status/343h8hq0wkymm42i?svg=true)](https://ci.appveyor.com/project/kensykora/musicbot-backend)

## Slack App Setup for local development

1. Create a new slack app (<https://api.slack.com/>)
1. Setup Icon, Description, etc.
1. Add a slash command: `https://api.slack.com/apps/<your app id>/slash-commands`

    Command: `/mb[-suffix]` where `-suffix` is optionally a suffix to specifically identify you. Useful when multiple bots in the same server.  
    Request Url: `<Base URL>/api/command` (e.g., `https://music-bot-dev-app.azurewebsites.net/api/command`)  
    Description: Control your music  
    Usage Hint: play, pause  
    Escape Channels: `checked`

1. Invite your bot to the server: `https://api.slack.com/apps/<your app id>/install-on-team`
1. Start your functions host (Open Visual Studio solution, build and run)
1. Start your reverse proxy `ngrok http 7071 --host-header=rewrite` (download and install <https://ngrok.com/> client)
1. Update your slash command above with the base URL from ngrok.
1. Update your `src/func/local.settings.json` file with your App's Verification Token (retrieve from <https://api.slack.com/apps/<YOUR APP ID>/general> 
1. Start invoking commands.

## Environment Variables

Setup these environment variables:

| Name                     | Description                                            | Example                                                                                                     |
| ------------------------ | ------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------- |
| `IoTHubConnectionString` | Connection string for iothubowner credential           | `HostName=ksykora-iot-free.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=4RSZw******A=` |
| `BetaKey`                | Just generate a random guid, doesn't matter what it is | `e7b27366-a893-45d3-bd84-faf1de87e8f7`                                                                      |
| `SlackVerificationToken` | Verification Token from your slack app homepage        | `3xandj83ndjkfPjndfDSdndf`                                                                                  |

## IoT Hub Setup for local development

1. Create an IoTHub Account (Free Tier) -- since you can only have 1 of these, you may want to use an existing one, or create a new one.
1. Stick an "owner" access policy connection string as an environment variable `IoTHubConnectionString` (then restart visual studio if it's open)

Note: This is potentially problematic for integration tests, as we cannot assume it is a good idea to clear out all registrations, so we do not reset IoT hub when integration tests run. You may occasionally encounter failures due to conflicts.

## Deployment

Dev and Prod are configured as github deployments to functions app. Thus, the linked template chooses not to include the functions deployment. For setting up dev and prod, manual deployment steps are required for pointing the functions app at the github for the `dev` and `prod` branches respectively.

Deployments to dev simply need to be passing builds.

Deployments to prod are triggered by tagging 'prod' and pushing to github.

## TODO:

Setup local db testing on appveyor