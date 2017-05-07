namespace NetTelegramBot.Framework
{
    public interface ICommandParser
    {
        ICommand TryParse(string message);
    }
}
