using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    public class TextEchoer : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat, "You said:\n" + msg.Text
            );

            await next(context);
        }
    }
}