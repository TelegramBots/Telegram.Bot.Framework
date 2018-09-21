using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Pipeline
{
    internal class MapMiddleware : IUpdateHandler
    {
        private readonly UpdateType _updateType;

        private readonly UpdateDelegate _branch;

        public MapMiddleware(UpdateType updateType, UpdateDelegate branch)
        {
            _updateType = updateType;
            _branch = branch;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (context.Update.Type == _updateType)
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
