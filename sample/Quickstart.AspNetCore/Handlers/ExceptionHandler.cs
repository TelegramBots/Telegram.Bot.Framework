using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    public class ExceptionHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            var u = context.Update;

            try
            {
                await next(context);
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