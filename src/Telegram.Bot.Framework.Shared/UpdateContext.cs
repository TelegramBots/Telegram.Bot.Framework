using Telegram.Bot.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    public class UpdateContext : IUpdateContext
    {
        public Update Update { get; }
        public bool IsWebhook { get; }
        public object HttpContext { get; }

        public UpdateContext(Update u)
        {
            Update = u;
            IsWebhook = false;
        }

        public UpdateContext(Update u, object httpContext)
        {
            Update = u;
            HttpContext = httpContext;
            IsWebhook = true;
        }
    }
}