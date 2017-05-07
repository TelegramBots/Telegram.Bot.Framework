namespace NetTelegramBot.Framework.Abstractions
{
    public interface IBotCommandArgs<TIBot>
        where TIBot : IBot
    {
        string RawInput { get; set; }
    }
}
