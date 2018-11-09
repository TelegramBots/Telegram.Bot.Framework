using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Base handler implementation for a command such as "/start"
    /// </summary>
    public abstract class CommandBase : IUpdateHandler
    {
        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args,
            CancellationToken cancellationToken);

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            return HandleAsync(context, next, ParseCommandArgs(context.Update.Message), cancellationToken);
        }

        public static string[] ParseCommandArgs(Message message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (message.Entities?.FirstOrDefault()?.Type != MessageEntityType.BotCommand)
                throw new ArgumentException("Message is not a command.", nameof(message));

            var argsList = new List<string>();
            string allArgs = message.Text.Substring(message.Entities[0].Length).TrimStart();
            argsList.Add(allArgs);

            var expandedArgs = Regex.Split(allArgs, @"\s+");
            if (expandedArgs.Length > 1)
            {
                argsList.AddRange(expandedArgs);
            }

            string[] args = argsList
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            return args;
        }
    }
}