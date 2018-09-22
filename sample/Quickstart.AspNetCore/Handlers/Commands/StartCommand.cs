using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    class StartCommand : CommandBase
    {
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat, "Hello, World!");
            await next(context);
        }
    }
}
