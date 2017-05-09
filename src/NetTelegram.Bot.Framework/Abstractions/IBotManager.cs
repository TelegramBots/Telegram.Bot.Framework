using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface IBotManager<TBot>
        where TBot : class, IBot
    {
        string WebhookRoute { get; }

        Task HandleUpdateAsync(Update update);

        Task GetAndHandleNewUpdatesAsync();

        Task SetWebhook(string appBaseUrl);
    }
}
