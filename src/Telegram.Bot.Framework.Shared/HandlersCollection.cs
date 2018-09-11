using System.Collections;
using System.Collections.Generic;
using Telegram.Bot.Abstractions;

namespace Telegram.Bot.Framework
{
    public class HandlersCollection<TBot> : IHandlersCollection<TBot>
        where TBot : IBot
    {
        private readonly IEnumerable<IHandlerPredicate> _handlerPredicates;

        public HandlersCollection(IEnumerable<IHandlerPredicate> handlers)
        {
            _handlerPredicates = handlers;
        }

        public IEnumerator<IHandlerPredicate> GetEnumerator() => _handlerPredicates.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}