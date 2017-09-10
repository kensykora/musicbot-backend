using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App
{
    public abstract class CommandBase<TResultType> : ICommand<TResultType> where TResultType : new()
    {
        public abstract Task<TResultType> ExecuteAsync();
    }
}