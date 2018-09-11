using Telegram.Bot.Types;

namespace Telegram.Bot.Abstractions
{
    public interface IUpdateContext
    {
        Update Update { get; }

        bool IsWebhook { get; }

        object HttpContext { get; }
    }
}