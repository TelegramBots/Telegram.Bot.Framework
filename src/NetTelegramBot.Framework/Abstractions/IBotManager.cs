using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IBotManager<TBot>
        where TBot : class, IBot
    {
        Task HandleUpdateAsync(Update update);

        Task GetAndHandleNewUpdatesAsync();
    }
}
