using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IUpdateHandler
    {
        IBot Bot { get; set; }

        bool CanHandle(Update update);

        Task<UpdateHandlingResult> HandleUpdateAsync(Update update);
    }
}
