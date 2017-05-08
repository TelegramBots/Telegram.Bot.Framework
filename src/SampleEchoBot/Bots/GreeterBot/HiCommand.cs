using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Bots.GreeterBot
{
    public interface IHiCommand : IBotCommand<IGreeterBot>
    {

    }

    public class HiCommandArgs : IBotCommandArgs<IGreeterBot>
    {
        public string RawInput { get; set; }

        public string PersonName { get; set; }
    }

    public class HiCommand : BotCommandBase<IGreeterBot, HiCommandArgs>, IHiCommand
    {
        private const string CommandName = "hi";

        private const string HiMessageFormat = "Hello, *{0}*!";

        public HiCommand()
            : base(CommandName)
        {

        }

        public override bool CanHandle(Update update)
        {
            var canHandle = false;
            if (!string.IsNullOrEmpty(update.Message.Text))
            {
                canHandle = Regex.IsMatch(update.Message.Text,
                    $@"^/{Name}(?:@{Bot.BotUserInfo.Username})?\s+\w+", RegexOptions.IgnoreCase);
            }
            return canHandle;
        }

        public override HiCommandArgs ParseInput(Update update)
        {
            var tokens = Regex.Split(update.Message.Text.Trim(), @"\s+");
            return new HiCommandArgs
            {
                RawInput = update.Message.Text,
                PersonName = tokens[1],
            };
        }

        public override async Task ProcessCommand(Update update, HiCommandArgs args)
        {
            var req = new SendMessage(update.Message.Chat.Id, string.Format(HiMessageFormat, args.PersonName))
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
