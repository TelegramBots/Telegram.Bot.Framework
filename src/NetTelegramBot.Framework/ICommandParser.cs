namespace NetTelegramBot.Framework
{
    using System;

    public interface ICommandParser
    {
        ICommand TryParse(string message);
    }
}
