using System.Threading.Tasks;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.Net45.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IBot bot, IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await bot.Client.AnswerCallbackQueryAsync(cq.Id, "PONG", showAlert: true);
        }
    }
}