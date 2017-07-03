namespace Telegram.Bot.Framework.Abstractions
{
    public interface IGameHandler : IUpdateHandler
    {
        string ShortName { get; }

        string BotBaseUrl { get; set; }

        string GamePageUrl { get; set; }
    }
}
