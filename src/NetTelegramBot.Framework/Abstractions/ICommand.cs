using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface ICommand<in TCommandArgs> : IUpdateHandler
        where TCommandArgs : ICommandArgs
    {
        /// <summary>
        /// Command name without leading '/'
        /// </summary>
        string Name { get; }

        Task HandleCommand(Update update, TCommandArgs commandArguments);
    }
}
