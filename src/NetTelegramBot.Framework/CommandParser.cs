namespace NetTelegramBot.Framework
{
    using System;
    using System.Linq;

    public class CommandParser : ICommandParser
    {
        private char mainDelimiter;
        private char[] additionalDelimiters;
        private char[] allDelimiters;

        public CommandParser()
            : this(false, null)
        {
            // Nothing
        }

        public CommandParser(bool convertCommandToUppercase)
            : this(convertCommandToUppercase, null)
        {
            // Nothing
        }

        public CommandParser(params char[] additionalDelimiters)
            : this(false, additionalDelimiters)
        {
            // Nothing
        }

        public CommandParser(bool convertCommandToLowercase, params char[] additionalDelimiters)
        {
            ConvertCommandToUppercase = convertCommandToLowercase;
            MainDelimiter = ' ';
            AdditionalDelimiters = additionalDelimiters;
        }

        public char CommandPrefix { get; set; } = '/';

        public char BotNameDelimiter { get; set; } = '@';

        public char MainDelimiter
        {
            get
            {
                return mainDelimiter;
            }

            set
            {
                mainDelimiter = value;
                allDelimiters = additionalDelimiters == null
                    ? new[] { mainDelimiter }
                    : new[] { mainDelimiter }.Concat(additionalDelimiters).ToArray();
            }
        }

        public char[] AdditionalDelimiters
        {
            get
            {
                return additionalDelimiters;
            }

            set
            {
                additionalDelimiters = value;
                allDelimiters = additionalDelimiters == null
                    ? new[] { mainDelimiter }
                    : new[] { mainDelimiter }.Concat(additionalDelimiters).ToArray();
            }
        }

        public bool ConvertCommandToUppercase { get; set; }

        public BotCommand TryParse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            if (message[0] != CommandPrefix)
            {
                return null;
            }

            var command = new BotCommand();

            var argsText = (string)null;

            var firstSpace = message.IndexOf(MainDelimiter);
            if (firstSpace == -1)
            {
                command.Command = message.Substring(1);
            }
            else
            {
                command.Command = message.Substring(1, firstSpace - 1); // -1 is for CommandPrefix compensation
                argsText = message.Substring(firstSpace + 1);
            }

            if (string.IsNullOrEmpty(command.Command) || command.Command[0] == BotNameDelimiter)
            {
                return null;
            }

            var botNameDelimiterIndex = command.Command.IndexOf(BotNameDelimiter);
            if (botNameDelimiterIndex != -1)
            {
                command.Bot = command.Command.Substring(botNameDelimiterIndex + 1);
                command.Command = command.Command.Substring(0, botNameDelimiterIndex);
            }

            if (additionalDelimiters != null && additionalDelimiters.Length != 0)
            {
                var array = command.Command.Split(additionalDelimiters, StringSplitOptions.RemoveEmptyEntries);
                command.Command = array[0];
                command.Args = array.Skip(1).ToArray();
            }

            if (argsText != null)
            {
                var array = argsText.Split(allDelimiters, StringSplitOptions.RemoveEmptyEntries);
                command.Args = command.Args == null
                    ? array
                    : command.Args.Concat(array).ToArray();
            }

            if (command.Args != null && command.Args.Length == 0)
            {
                command.Args = null;
            }

            if (ConvertCommandToUppercase)
            {
                command.Command = command.Command.ToUpperInvariant();
            }

            return command;
        }
    }
}
