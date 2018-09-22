using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// A Telegram bot command such as /start
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