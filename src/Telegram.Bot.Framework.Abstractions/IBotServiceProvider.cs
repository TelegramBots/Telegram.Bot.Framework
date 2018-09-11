using System;

namespace Telegram.Bot.Abstractions
{
    public interface IBotServiceProvider<TBot> : IDisposable
        where TBot : class, IBot
    {
        IBotServiceProvider<TBot> CreateScope();

        TBot GetBot();

        bool TryGetBotOptions(out IBotOptions options);

        IHandlersCollection<TBot> GetHandlersCollection();

        IUpdateHandler GetHandler(Type t);
    }
}