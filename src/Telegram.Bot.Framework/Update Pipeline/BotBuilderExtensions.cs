using System;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework
{
    public static class BotBuilderExtensions
    {
        public static IBotBuilder UseWhen(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        )
        {
            var branchBuilder = new BotBuilder();
            configure(branchBuilder);
            UpdateDelegate branchDelegate = branchBuilder.Build();

            builder.Use(new UseWhenMiddleware(predicate, branchDelegate));

            return builder;
        }

        public static IBotBuilder UseWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate
        )
            where THandler : IUpdateHandler
        {
            var branchDelegate = new BotBuilder().Use<THandler>().Build();
            builder.Use(new UseWhenMiddleware(predicate, branchDelegate));
            return builder;
        }

        [Obsolete]
        public static IBotBuilder Map(
            this IBotBuilder builder,
            string updateType,
            Action<IBotBuilder> configure
        )
        {
            var mapBuilder = new BotBuilder();
            configure(mapBuilder);
            UpdateDelegate mapDelegate = mapBuilder.Build();

            builder.Use(new MapMiddleware(updateType, mapDelegate));

            return builder;
        }

        public static IBotBuilder MapWhen(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        )
        {
            var mapBuilder = new BotBuilder();
            configure(mapBuilder);
            UpdateDelegate mapDelegate = mapBuilder.Build();

            builder.Use(new MapWhenMiddleware(predicate, mapDelegate));

            return builder;
        }

        public static IBotBuilder MapWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate
        )
            where THandler : IUpdateHandler
        {
            var branchDelegate = new BotBuilder().Use<THandler>().Build();
            builder.Use(new MapWhenMiddleware(predicate, branchDelegate));
            return builder;
        }

        public static IBotBuilder UseCommand<TCommand>(
            this IBotBuilder builder,
            string command
        )
            where TCommand : CommandBase
            => builder
                .MapWhen(
                    ctx => ctx.Bot.CanHandleCommand(command, ctx.Update.Message),
                    botBuilder => botBuilder.Use<TCommand>()
                );
    }
}
