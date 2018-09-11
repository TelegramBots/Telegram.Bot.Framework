using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Framework;

namespace Quickstart.Net45.Services.SimpleInjector
{
    class BotServiceProvider<TBot> : IBotServiceProvider<TBot>
        where TBot : class, IBot
    {
        private readonly Container _container;

        private readonly Scope _scope;

        public BotServiceProvider(Container container)
        {
            _container = container;
        }

        private BotServiceProvider(Scope scope)
        {
            _scope = scope;
        }

        public IBotServiceProvider<TBot> CreateScope() =>
            new BotServiceProvider<TBot>(ThreadScopedLifestyle.BeginScope(_container));

        public TBot GetBot() => (_scope?.Container ?? _container).GetInstance<TBot>();

        public bool TryGetBotOptions(out IBotOptions options)
        {
            var reg = (_scope?.Container ?? _container).GetCurrentRegistrations()
                .SingleOrDefault(x => x.ServiceType == typeof(BotOptions<TBot>));
            if (reg != null)
            {
                options = (IBotOptions)reg.GetInstance();
            }
            else
            {
                options = null;
            }

            return options != null;
        }

        public IHandlersCollection<TBot> GetHandlersCollection() =>
            (_scope?.Container ?? _container).GetInstance<IHandlersCollection<TBot>>();

        public IUpdateHandler GetHandler(Type t) =>
            (IUpdateHandler)(_scope?.Container ?? _container).GetInstance(t);

        public void Dispose() => _scope?.Dispose();
    }
}
