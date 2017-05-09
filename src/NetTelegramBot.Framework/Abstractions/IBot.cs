using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IBot
    {
        User BotUserInfo { get; }

        WebhookInfo WebhookInfo { get; }

        Task<T> MakeRequestAsync<T>(NetTelegramBotApi.Requests.RequestBase<T> request);
    }
}
