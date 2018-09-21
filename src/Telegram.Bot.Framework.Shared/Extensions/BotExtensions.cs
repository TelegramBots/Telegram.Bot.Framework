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

            {
                bool isTextMessage = message?.Text != null;
                if (!isTextMessage)
                    return false;
            }

            {
                bool isCommand = message.Entities.FirstOrDefault()?.Type == MessageEntityType.BotCommand;
                if (!isCommand)
                    return false;
            }

            return Regex.IsMatch(
                message.Text,
                $@"^/{commandName}(?:@{bot.Username})?(?:\s+|$)",
                RegexOptions.IgnoreCase
            );
        }
    }
}
