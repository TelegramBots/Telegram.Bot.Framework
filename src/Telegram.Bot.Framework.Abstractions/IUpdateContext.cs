using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IUpdateContext
    {
        IBot Bot { get; }

        Update Update { get; }

        bool IsWebhook { get; }

        object HttpContext { get; }

        IServiceProvider Services { get; }

        IDictionary<string, object> Items { get; }
    }
}