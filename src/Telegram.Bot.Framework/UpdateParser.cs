using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Responsible for quickly parsing the updates and finding the handlers for them
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public class UpdateParser<TBot> : IUpdateParser<TBot>
        where TBot : BotBase<TBot>
    {
        /// <summary>
        /// List of all available 
        /// </summary>
        protected IEnumerable<IUpdateHandler> UpdateHandlers => _handlersAccessor.UpdateHandlers;

        private readonly IUpdateHandlersAccessor<TBot> _handlersAccessor;

        /// <summary>
        /// Initializes and instance with the provided handler acessor
        /// </summary>
        /// <param name="handlersAccessor">Update handler accessor for the bot</param>
        public UpdateParser(IUpdateHandlersAccessor<TBot> handlersAccessor)
        {
            _handlersAccessor = handlersAccessor;
        }

        /// <summary>
        /// Finds update handlers for this specific bot update
        /// </summary>
        /// <param name="bot">Instance of bot</param>
        /// <param name="update">Update to be checked for handling</param>
        /// <returns>List of update handlers for the bot able to handle that update</returns>
        public virtual IEnumerable<IUpdateHandler> FindHandlersForUpdate(IBot bot, Update update)
        {
            IEnumerable<IUpdateHandler> handlers = UpdateHandlers
                .Where(x => x.CanHandleUpdate(bot, update));

            return handlers;
        }
    }
}
