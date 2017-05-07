using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using SampleEchoBot.Services;

namespace SampleEchoBot.Commands
{
    public interface IStartCommand : IBotCommand<IEchoBot>
    {

    }

    public class StartCommandArgs : IBotCommandArgs<IEchoBot>
    {
        public string RawInput { get; set; }
    }

    public class StartCommand : BotCommandBase<IEchoBot, StartCommandArgs>, IStartCommand
    {
        private const string CommandName = "start";

        public StartCommand()
            : base(CommandName)
        {

        }

        public override async Task ProcessCommand(Update update, StartCommandArgs args)
        {
            var req = new SendMessage(update.Message.Chat.Id, $"Hi {update.Message.From.FirstName}!");
            await Bot.MakeRequestAsync(req);
        }
    }
}
