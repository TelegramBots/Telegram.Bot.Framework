namespace NetTelegramBot.Framework.Model
{
    using System;

    public abstract class EntityKey
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
    }
}
