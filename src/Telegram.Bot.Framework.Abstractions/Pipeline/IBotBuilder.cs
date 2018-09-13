using System;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IBotBuilder
    {
        IBotBuilder Use(Func<IUpdateContext, UpdateDelegate> component);

        IBotBuilder Use<THandler>()
            where THandler : IUpdateHandler;

        IBotBuilder UseWhen(
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        );

        IBotBuilder Map(
            UpdateType type,
            Action<IBotBuilder> configure
        );

        IBotBuilder MapWhen(
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        );

        UpdateDelegate Build();
    }
}
