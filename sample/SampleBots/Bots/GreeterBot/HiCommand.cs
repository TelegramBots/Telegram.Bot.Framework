using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SampleBots.Bots.GreeterBot
{
    public class HiCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }

        public string PersonName { get; set; }
    }

    public class HiCommand : CommandBase<HiCommandArgs>
    {
        private const string CommandName = "hi";

        private const string HiMessageFormat = "Hello, *{0}*!";

        private const string HelpText = "Here is a tip to use this command:\n" +
                                        "```\n" +
                                        "/hi John" +
                                        "```";

        public HiCommand()
            : base(CommandName)
        {

        }

        protected override HiCommandArgs ParseInput(Update update)
        {
            var tokens = Regex.Split(update.Message.Text.Trim(), @"\s+");
            var args = base.ParseInput(update);

            if (tokens.Length > 1)
            {
                args.PersonName = string.Join(" ", tokens.Skip(1));
            }

            return args;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, HiCommandArgs args)
        {
            string text;
            if (args.PersonName is null)
            {
                text = HelpText;
            }
            else
            {
                text = string.Format(HiMessageFormat, args.PersonName);
            }

            await Bot.Client.SendTextMessageAsync(
                update.Message.Chat.Id,
                text,
                ParseMode.Markdown,
                replyToMessageId: update.Message.MessageId
                );

            return UpdateHandlingResult.Continue;
        }
    }
}
