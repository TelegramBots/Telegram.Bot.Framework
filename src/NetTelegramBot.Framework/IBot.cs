using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public interface IBot
    {
        User BotUserInfo { get; }

        Task<T> MakeRequestAsync<T>(NetTelegramBotApi.Requests.RequestBase<T> request);

        Task ProcessUpdateAsync(Update update);
    }
}
