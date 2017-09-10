using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App.Commands
{
    public abstract class CommandBase<TResultType> : ICommand<TResultType> where TResultType : class
    {
        public abstract Task<TResultType> ExecuteAsync();
    }
}