using System.Threading.Tasks;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    public abstract class UpdateHandlerBase : IUpdateHandler
    {
        public abstract bool CanHandleUpdate(IBot bot, Update update);

        public abstract Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update);
    }
}
