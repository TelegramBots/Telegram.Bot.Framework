using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.Net45.Services.SimpleInjector
{
    class BotServiceProvider : IBotServiceProvider
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

        public object GetService(Type serviceType) =>
            _scope != null
                ? _scope.GetInstance(serviceType)
                : _container.GetInstance(serviceType)
        ;

        public IBotServiceProvider CreateScope() =>
            new BotServiceProvider(ThreadScopedLifestyle.BeginScope(_container));

        public void Dispose()
        {
            _scope?.Dispose();
            _container?.Dispose();
        }
    }
}
