using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class UpdateHandlerBase : IUpdateHandler
    {
        public abstract bool CanHandleUpdate(IBot bot, Update update);

        public abstract Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update);
    }
}
