using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Processes an update
    /// </summary>
    public interface IUpdateHandler
    {
        bool CanHandleUpdate(IBot bot, Update update);

        Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update);
    }
}
