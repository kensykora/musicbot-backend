using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using MusicBot.Functions.Models;

namespace MusicBot.Functions.SlashCommands
{
    public class MusicBotCommandFactory
    {
        private static readonly Dictionary<string, Type> _registeredCommands = GetEnumerableOfType<MusicBotCommand>();

        public static MusicBotCommand GetCommand(SlackSlashCommandRequest req)
        {
            var key = _registeredCommands.Keys.FirstOrDefault(x =>
                x.Equals(req.Command, StringComparison.OrdinalIgnoreCase));

            if (key == null)
            {
                throw new ApplicationException("Unable to determine command type of request");
            }

            return (MusicBotCommand)Activator.CreateInstance(_registeredCommands[key], req);
        }

        public static Dictionary<string, Type> GetEnumerableOfType<T>() where T : class
        {
            var result = new Dictionary<string, Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T))))
            {
                result.Add(type.GetMethod(nameof(MusicBotCommand.ExpectedCommandType)).Invoke(null, null).ToString(), type);
            }

            return result;
        }
    }
}
