using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface IBot
    {
        User BotUserInfo { get; }

        WebhookInfo WebhookInfo { get; }

        Task<T> MakeRequestAsync<T>(NetTelegramBotApi.Requests.RequestBase<T> request);
    }
}
