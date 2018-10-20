using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    class UpdateMembersList : IUpdateHandler
    {
        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Updating chat members list...");
            Console.ResetColor();

            return next(context);
        }
    }
}
