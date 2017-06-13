using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetTelegram.Bot.Framework;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.GreeterBot
{
    public class HiCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string PersonName { get; set; }
    }

    public class HiCommand : CommandBase<HiCommandArgs>
    {
        private const string CommandName = "hi";

        private const string HiMessageFormat = "Hello, *{0}*!";

        public HiCommand()
            : base(CommandName)
        {

        }

        protected override bool CanHandleCommand(Update update)
        {
            var canHandle = false;
            if (!string.IsNullOrEmpty(update.Message.Text))
            {
                canHandle = Regex.IsMatch(update.Message.Text,
                    $@"^/{Name}(?:@{Bot.BotUserInfo.Username})?\s+\w+", RegexOptions.IgnoreCase);
            }
            return canHandle;
        }

        protected override HiCommandArgs ParseInput(Update update)
        {
            var tokens = Regex.Split(update.Message.Text.Trim(), @"\s+");
            return new HiCommandArgs
            {
                RawInput = update.Message.Text,
                PersonName = tokens[1],
            };
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, HiCommandArgs args)
        {
            var req = new SendMessage(update.Message.Chat.Id, string.Format(HiMessageFormat, args.PersonName))
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequest(req);
            return UpdateHandlingResult.Continue;
        }
    }
}
