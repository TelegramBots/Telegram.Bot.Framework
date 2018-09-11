using System;

namespace Telegram.Bot.Abstractions
{
    public interface IHandlerPredicate
    {
        Type Type { get; }

        Func<IBot, IUpdateContext, bool> CanHandle { get; }
    }
}
