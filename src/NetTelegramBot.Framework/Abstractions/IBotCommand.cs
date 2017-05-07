using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IBotCommand<TIBot> : IMessageHandler<TIBot>
        where TIBot : IBot
    {
        /// <summary>
        /// Command name without leading '/'
        /// </summary>
        string Name { get; }

        Task HandleCommand(Update update, IBotCommandArgs<TIBot> commandArguments);
    }
}
