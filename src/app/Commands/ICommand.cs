using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App.Commands
{
    public interface ICommand<TResultType> where TResultType : class
    {
        Task<TResultType> ExecuteAsync();
    }
}