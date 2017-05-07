using System.Threading.Tasks;
using NetTelegramBotApi.Types;
using RecurrentTasks;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IBotUpdateManager<TBot> : IRunnable
        where TBot : IBot
    {
        Task HandleUpdateAsync(Update update);
    }
}
