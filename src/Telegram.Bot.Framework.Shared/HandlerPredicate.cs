using System;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public struct HandlerPredicate : IHandlerPredicate
    {
        public Type Type { get; }

        public Func<IBot, IUpdateContext, bool> CanHandle { get; }

        public HandlerPredicate(Type t, Func<IBot, IUpdateContext, bool> predicate)
        {
            Type = t;
            CanHandle = predicate;
        }
    }
}
