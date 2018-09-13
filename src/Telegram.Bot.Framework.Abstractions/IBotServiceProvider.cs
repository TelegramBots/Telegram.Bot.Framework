using System;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IBotServiceProvider : IServiceProvider, IDisposable
    {
        IBotServiceProvider CreateScope();
    }
}
