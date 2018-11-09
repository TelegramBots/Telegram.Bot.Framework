using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public static class BotExtensions
    {
        public static bool CanHandleCommand(this IBot bot, string commandName, Message message)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("Invalid command name", nameof(commandName));
            if (commandName.StartsWith("/"))
                throw new ArgumentException("Command name must not start with '/'.", nameof(commandName));

            if (message is null)
                return false;

            {
                bool isTextMessage = message.Text != null;
                if (!isTextMessage)
                    return false;
            }

            {
                bool isCommand = message.Entities?.FirstOrDefault()?.Type == MessageEntityType.BotCommand;
                if (!isCommand)
                    return false;
            }

            return Regex.IsMatch(
                message.EntityValues.First(),
                $@"^/{commandName}(?:@{bot.Username})?$",
                RegexOptions.IgnoreCase
            );
        }
    }
}