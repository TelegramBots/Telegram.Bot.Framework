using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.Net45.Handlers
{
    class WebhookLogger : IUpdateHandler
    {
        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Received update {0} as a webhook.", context.Update.Id);
            Console.ResetColor();

            return next(context, cancellationToken);
        }
    }
}