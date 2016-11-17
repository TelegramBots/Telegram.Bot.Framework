namespace NetTelegramBot.Framework
{
    using System;

    public interface ICommandParser
    {
        BotCommand TryParse(string message);
    }
}
