using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace SampleGames.Bots.CrazyCircle
{
    public class StartCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }
    }

    public class StartCommand : CommandBase<StartCommandArgs>
    {
        public StartCommand()
            : base(Constants.CommandName)
        {

        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, StartCommandArgs args)
        {
            await Bot.Client.SendGameAsync(update.Message.Chat.Id, "crazycircle");
            
            return UpdateHandlingResult.Handled;
        }

        private static class Constants
        {
            public const string CommandName = "start";
        }
    }
}
