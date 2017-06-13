using System.Threading.Tasks;
using NetTelegram.Bot.Framework;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.GreeterBot
{
    public class StartCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }
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
            var req = new SendMessage(update.Message.Chat.Id, string.Format(StartMessageFormat, update.Message.From.FirstName))
            {
                ReplyToMessageId = update.Message.ForwardFromMessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequest(req);
            return UpdateHandlingResult.Handled;
        }
    }
}
