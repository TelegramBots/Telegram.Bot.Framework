using System.Threading.Tasks;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.Net45.Handlers.Commands
{
    class Ping : CommandBase
    {
        public override async Task HandleAsync(IBot bot, IUpdateContext context, UpdateDelegate next, string[] args)
        {
            Message msg = context.Update.Message;

            await bot.Client.SendTextMessageAsync(
                msg.Chat,
                "*PONG*",
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
                )
            );
        }
    }
}
