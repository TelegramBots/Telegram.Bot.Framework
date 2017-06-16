using System.Threading.Tasks;
using NetTelegram.Bot.Framework;
using NetTelegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NetTelegram.Bot.Sample.Bots.GreeterBot
{
    public class StartCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }
    }

    public class StartCommand : CommandBase<StartCommandArgs>
    {
        private const string CommandName = "start";

        private const string StartMessageFormat = "Hey *{0}*!\n\n" +
                                                     "Try my _hi_ command like this:\n" +
                                                     "`/hi John`\n\n" +
                                                     "By the way, I _forward back any photo you send_ here ;)";

        public StartCommand()
            : base(CommandName)
        {

        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, StartCommandArgs args)
        {
            await Bot.Client.SendTextMessageAsync(update.Message.Chat.Id,
                string.Format(StartMessageFormat, update.Message.From.FirstName),
                ParseMode.Markdown,
                replyToMessageId: update.Message.ForwardFromMessageId);

            return UpdateHandlingResult.Handled;
        }
    }
}
