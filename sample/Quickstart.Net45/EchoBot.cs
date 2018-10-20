using Telegram.Bot.Framework;

namespace Quickstart.Net45
{
    class EchoBot : BotBase
    {
        public EchoBot(string apiToken)
            : base(username: "quickstart_tgbot", token: apiToken)
        {
        }
    }
}
