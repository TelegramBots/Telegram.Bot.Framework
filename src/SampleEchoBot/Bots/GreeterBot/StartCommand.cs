using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Bots.GreeterBot
{
    public interface IStartCommand : IBotCommand<IGreeterBot>
    {

    }

    public class StartCommandArgs : IBotCommandArgs<IGreeterBot>
    {
        public string RawInput { get; set; }
    }

    public class StartCommand : BotCommandBase<IGreeterBot, StartCommandArgs>, IStartCommand
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

        public override async Task ProcessCommand(Update update, StartCommandArgs args)
        {
            var req = new SendMessage(update.Message.Chat.Id, string.Format(StartMessageFormat, update.Message.From.FirstName))
            {
                ReplyToMessageId = update.Message.ForwardFromMessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
