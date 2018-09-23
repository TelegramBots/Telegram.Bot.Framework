using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Pipeline
{
    [Obsolete]
    internal class MapMiddleware : IUpdateHandler
    {
        private readonly string _updateType;

        private readonly UpdateDelegate _branch;

        public MapMiddleware(string updateType, UpdateDelegate branch)
        {
            _updateType = updateType;
            _branch = branch;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (context.Update.IsOfType(_updateType))
            {
                return _branch(context);
            }
            else
            {
                return next(context);
            }
        }
    }
}
