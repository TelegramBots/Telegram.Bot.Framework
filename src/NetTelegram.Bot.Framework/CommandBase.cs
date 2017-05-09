using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    public abstract class CommandBase<TCommandArgs> : ICommand<TCommandArgs>
        where TCommandArgs : ICommandArgs, new()
    {
        /// <summary>
        /// Command name without leading '/'
        /// </summary>
        public string Name { get; }

        protected IBot Bot { get; private set; }

        protected CommandBase(string name)
        {
            Name = name;
        }

        public bool CanHandleUpdate(IBot bot, Update update)
        {
            Bot = Bot ?? bot;
            return CanHandleCommand(update);
        }

        public async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            Bot = Bot ?? bot;
            var args = ParseInput(update);
            return await HandleCommand(update, args);
        }

        protected virtual TCommandArgs ParseInput(Update update)
        {
            return new TCommandArgs { RawInput = update.Message.Text };
        }

        protected virtual bool CanHandleCommand(Update update)
        {
            var canHandle = false;
            if (!string.IsNullOrEmpty(update.Message.Text))
            {
                canHandle = Regex.IsMatch(update.Message.Text,
                    $@"^\s*/{Name}(?:(?:@{Bot.BotUserInfo.Username}(?:\s+.*)?)|\s+.*|)\s*$", RegexOptions.IgnoreCase);
            }
            return canHandle;
        }

        public abstract Task<UpdateHandlingResult> HandleCommand(Update update, TCommandArgs args);
    }
}
