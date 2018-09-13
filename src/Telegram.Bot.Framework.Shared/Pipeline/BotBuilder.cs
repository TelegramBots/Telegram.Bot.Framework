using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class BotBuilder<TBot> : IBotBuilder
        where TBot : IBot
    {
        internal UpdateDelegate UpdateDelegateChain { get; private set; }

        private readonly ICollection<Func<UpdateDelegate, UpdateDelegate>> _components;

        public BotBuilder()
        {
            _components = new List<Func<UpdateDelegate, UpdateDelegate>>();
        }

        public IBotBuilder Use<THandler>()
            where THandler : IUpdateHandler
        {
            _components.Add(
                next =>
                context =>
                ((IUpdateHandler)context.Services.GetService(typeof(THandler)))
                    .HandleAsync(context, next)
            );

            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler)
            where THandler : IUpdateHandler
        {
            _components.Add(next =>
                context => handler.HandleAsync(context, next)
            );

            return this;
        }

        public IBotBuilder UseWhen(Predicate<IUpdateContext> predicate, Action<IBotBuilder> configure)
        {
            var branchBuilder = new BotBuilder<TBot>();
            configure(branchBuilder);
            UpdateDelegate branchDelegate = branchBuilder.Build();

            Use(new UseWhenMiddleware(predicate, branchDelegate));

            return this;
        }

        public IBotBuilder Map(UpdateType type, Action<IBotBuilder> configure)
        {
            var mapBuilder = new BotBuilder<TBot>();
            configure(mapBuilder);
            UpdateDelegate mapDelegate = mapBuilder.Build();

            Use(new MapMiddleware(type, mapDelegate));

            return this;
        }

        public IBotBuilder MapWhen(Predicate<IUpdateContext> predicate, Action<IBotBuilder> configure)
        {
            var mapBuilder = new BotBuilder<TBot>();
            configure(mapBuilder);
            UpdateDelegate mapDelegate = mapBuilder.Build();

            Use(new MapWhenMiddleware(predicate, mapDelegate));

            return this;
        }

        public IBotBuilder Use(Func<IUpdateContext, UpdateDelegate> component)
        {
            throw new NotImplementedException();
        }

        public UpdateDelegate Build()
        {
            UpdateDelegate handle = context => Task.FromResult(null as object);
            foreach (var component in _components.Reverse())
            {
                handle = component(handle);
            }

            return UpdateDelegateChain = handle;
        }
    }
}
