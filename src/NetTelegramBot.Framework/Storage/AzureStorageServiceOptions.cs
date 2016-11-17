namespace NetTelegramBot.Framework.Storage
{
    public class AzureStorageServiceOptions
    {
        public string ConnectionString { get; set; }

        public string MainTableName { get; set; } = "BotData";

        public string LogTablePrefix { get; set; } = "ChatLog";
    }
}
