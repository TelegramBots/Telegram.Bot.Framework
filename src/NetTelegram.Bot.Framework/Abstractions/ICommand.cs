using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface ICommand<in TCommandArgs> : IUpdateHandler
        where TCommandArgs : ICommandArgs
    {
        /// <summary>
        /// Command name without leading '/'
        /// </summary>
        string Name { get; }

        Task<UpdateHandlingResult> HandleCommand(Update update, TCommandArgs args);
    }
}
