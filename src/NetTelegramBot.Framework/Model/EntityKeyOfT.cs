namespace NetTelegramBot.Framework.Model
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class EntityKey<T> : EntityKey
        where T : ITableEntity
    {
        // Nothing more
    }
}
