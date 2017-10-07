using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MusicBot.Functions.Models;

namespace MusicBot.Functions.SlashCommands
{
    public class MusicBotCommandFactory
    {
        private static readonly Dictionary<string, Type> RegisteredCommands = GetEnumerableOfType<MusicBotCommand>();

        public static MusicBotCommand GetCommand(SlackSlashCommandRequest req)
        {
            var key = RegisteredCommands.Keys.FirstOrDefault(x =>
                x.Equals(req.Text?.Split(' ').FirstOrDefault(), StringComparison.OrdinalIgnoreCase));

            if (key == null)
            {
                return null;
            }

            return (MusicBotCommand)Activator.CreateInstance(RegisteredCommands[key], req);
        }

        public static Dictionary<string, Type> GetEnumerableOfType<T>() where T : class
        {
            var result = new Dictionary<string, Type>();
            foreach (var type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T))))
            {
                var instance = (MusicBotCommand)Activator.CreateInstance(type);
                result.Add(instance.CommandType, type);
            }

            return result;
        }
    }
}
