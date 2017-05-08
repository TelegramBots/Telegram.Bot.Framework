using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class UpdateHandlerBase : IUpdateHandler
    {
        public IBot Bot { get; set; }

        public abstract bool CanHandle(Update update);

        public abstract Task HandleUpdateAsync(Update update);
    }
}
