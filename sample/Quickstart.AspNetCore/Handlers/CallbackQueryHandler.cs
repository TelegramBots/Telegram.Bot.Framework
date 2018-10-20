using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.AnswerCallbackQueryAsync(cq.Id, "PONG", showAlert: true);

            await next(context);
        }
    }
}