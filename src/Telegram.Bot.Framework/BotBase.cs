using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public abstract class BotBase : IBot
    {
        public ITelegramBotClient Client { get; }

        public string Username { get; }

        protected BotBase(string username, ITelegramBotClient client)
        {
            Username = username;
            Client = client;
        }

        protected BotBase(string username, string token)
            : this(username, new TelegramBotClient(token))
        {
        }

        protected BotBase(IBotOptions options)
            : this(options.Username, new TelegramBotClient(options.ApiToken))
        {
        }
    }
}