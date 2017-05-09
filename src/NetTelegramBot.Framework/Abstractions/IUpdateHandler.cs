using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IUpdateHandler
    {
        bool CanHandleUpdate(IBot bot, Update update);

        Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update);
    }
}
