using System;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IHandlerPredicate
    {
        Type Type { get; }

        Func<IBot, IUpdateContext, bool> CanHandle { get; }
    }
}
