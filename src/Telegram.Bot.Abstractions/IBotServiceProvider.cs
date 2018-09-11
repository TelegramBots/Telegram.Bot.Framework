using System;

namespace Telegram.Bot.Abstractions
{
    public interface IBotServiceContainer<TBot>
        where TBot : class, IBot
    {
        IBotServiceContainer<TBot> Bot(Func<TBot> instanceCreator);

        IBotServiceContainer<TBot> Use<THandler>()
            where THandler : class, IUpdateHandler;

        IBotServiceContainer<TBot> Use<THandler>(Func<THandler> creator)
            where THandler : class, IUpdateHandler;

        IBotUpdateManager<TBot> Register();
    }
}