using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot.App
{
    public interface ICommand<TResultType> where TResultType : new()
    {
        Task<TResultType> ExecuteAsync();
    }
}