using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    public class UpdateContext : IUpdateContext
    {
        public IBot Bot { get; }

        public Update Update { get; }

        public IServiceProvider Services { get; }

        public IDictionary<string, object> Items { get; }

        public UpdateContext(IBot bot, Update u, IServiceProvider services)
        {
            Bot = bot;
            Update = u;
            Services = services;
            Items = new ConcurrentDictionary<string, object>();
        }
    }
}