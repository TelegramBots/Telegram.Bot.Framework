using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Base class for implementing Bots
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public abstract class BotBase<TBot> : IBot
        where TBot : class, IBot
    {
        /// <summary>
        /// Gets Bot's user name
        /// </summary>
        public string UserName => BotOptions.BotUserName;

        /// <summary>
        /// Instance of Telegram bot client
        /// </summary>
        public ITelegramBotClient Client { get; }

        /// <summary>
        /// Options used to the configure the bot instance
        /// </summary>
        protected BotOptions<TBot> BotOptions { get; }

        /// <summary>
        /// Initializes a new Bot
        /// </summary>
        /// <param name="botOptions">Options used to configure the bot</param>
        protected BotBase(IOptions<BotOptions<TBot>> botOptions)
        {
            BotOptions = botOptions.Value;
            Client = new TelegramBotClient(BotOptions.ApiToken);
        }

        /// <summary>
        /// Responsible for handling bot updates that don't have any handler
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public abstract Task HandleUnknownUpdate(Update update);

        /// <summary>
        /// Receives the update when the hanlding process throws an exception for the update
        /// </summary>
        /// <param name="update"></param>
        /// <param name="e">Exception thrown while processing the update</param>
        /// <returns></returns>
        public abstract Task HandleFaultedUpdate(Update update, Exception e);
    }
}
