using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.Net45.Handlers
{
    public class ExceptionHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var u = context.Update;

            try
            {
                await next(context, cancellationToken);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occured in handling update {0}.{1}{2}", u.Id, Environment.NewLine, e);
                Console.ResetColor();
            }
        }
    }
}