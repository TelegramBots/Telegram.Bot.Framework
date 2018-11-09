using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public class BotBuilder : IBotBuilder
    {
        internal UpdateDelegate UpdateDelegate { get; private set; }

        private readonly ICollection<Func<UpdateDelegate, UpdateDelegate>> _components;

        public BotBuilder()
        {
            _components = new List<Func<UpdateDelegate, UpdateDelegate>>();
        }

        public IBotBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        public IBotBuilder Use<THandler>()
            where THandler : IUpdateHandler
        {
            _components.Add(
                next =>
                    (context, cancellationToken) =>
                    {
                        if (context.Services.GetService(typeof(THandler)) is IUpdateHandler handler)
                            return handler.HandleAsync(context, next, cancellationToken);
                        else
                            throw new NullReferenceException(
                                $"Unable to resolve handler of type {typeof(THandler).FullName}"
                            );
                    }
            );

            return this;
        }

        public IBotBuilder Use(UpdateDelegate component)
        {
            _components.Add(next => component);

            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler)
            where THandler : IUpdateHandler
        {
            _components.Add(next =>
                (context, cancellationToken) => handler.HandleAsync(context, next, cancellationToken)
            );

            return this;
        }

        public UpdateDelegate Build()
        {
            UpdateDelegate handle = (context, cancellationToken) =>
            {
                // use Logger
                Console.WriteLine("No handler for update {0} of type {1}.", context.Update.Id, context.Update.Type);
                return Task.FromResult(1);
            };

            foreach (var component in _components.Reverse())
            {
                handle = component(handle);
            }

            return UpdateDelegate = handle;
        }
    }
}