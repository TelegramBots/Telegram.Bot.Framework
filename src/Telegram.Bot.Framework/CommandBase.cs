using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Base handler implementation for a command such as "/start"
    /// </summary>
    public abstract class CommandBase : IUpdateHandler
    {
        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args);

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            // ToDo parse args
            return HandleAsync(context, next, new string[0]);
        }
    }
}