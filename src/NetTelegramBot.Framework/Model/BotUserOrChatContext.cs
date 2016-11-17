namespace NetTelegramBot.Framework.Model
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using NetTelegramBotApi.Types;

    public class BotUserOrChatContext : TableEntity
    {
        private const int CurrentSerializationVersion = 1;

        [Obsolete("This constructor is for Azure infrastructure only!")]
        public BotUserOrChatContext()
        {
            // Nothing
        }

        public BotUserOrChatContext(long botId, long userOrChatId, ChatType chatType, string value)
        {
            BotId = botId;
            UserOrChatId = userOrChatId;
            ChatType = chatType;
            Value = value;
            Version = DateTimeOffset.UtcNow;

            var key = GetKey(botId, userOrChatId);
            PartitionKey = key.PartitionKey;
            RowKey = key.RowKey;
        }

        public long BotId { get; set; }

        public long UserOrChatId { get; set; }

        public ChatType ChatType { get; set; }

        public string Value { get; set; }

        public DateTimeOffset Version { get; set; }

        public static EntityKey<BotUserOrChatContext> GetKey(long botId, long userOrChatId)
        {
            return new EntityKey<BotUserOrChatContext>
            {
                PartitionKey = nameof(BotUserOrChatContext) + "_" + botId,
                RowKey = userOrChatId.ToString()
            };
        }

        public static TableQuery<BotUserOrChatContext> GetAllForBot(long botId)
        {
            var condition1 = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                nameof(BotUserOrChatContext) + "_" + botId);

            return new TableQuery<BotUserOrChatContext>
            {
                FilterString = condition1
            };
        }

        public static TableQuery<BotUserOrChatContext> GetAllForBot(long botId, ChatType chatType)
        {
            var condition1 = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                nameof(BotUserOrChatContext) + "_" + botId);

            var condition2 = TableQuery.GenerateFilterConditionForInt(
                nameof(ChatType),
                QueryComparisons.Equal,
                (int)chatType);

            return new TableQuery<BotUserOrChatContext>
            {
                FilterString = TableQuery.CombineFilters(condition1, TableOperators.And, condition2)
            };
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            //// base.ReadEntity(properties, operationContext); - there is no any 'base' properties

            var sourceEntityType = properties[EntityPropertyExtensions.EntityTypePropertyName].StringValue;
            if (!string.Equals(nameof(BotUserOrChatContext), sourceEntityType))
            {
                throw new Exception($"Can't restore from entity type {sourceEntityType}");
            }

            var serializationVersion = properties[EntityPropertyExtensions.SerializationVersionPropertyName].Int32Value.Value;
            if (serializationVersion > CurrentSerializationVersion)
            {
                throw new Exception($"Can't restore from serialization version {serializationVersion}");
            }

            BotId = properties[nameof(BotId)].Int64Value.Value;
            UserOrChatId = properties[nameof(UserOrChatId)].Int64Value.Value;
            ChatType = (ChatType)properties[nameof(ChatType)].Int32Value.Value;
            Value = properties.ContainsKey(nameof(Value))
                ? properties[nameof(Value)].StringValue
                : null;
            Version = properties[nameof(Version)].DateTimeOffsetValue.Value;
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            //// var dic = base.WriteEntity(operationContext); - there is no any 'base' properties

            var dic = new Dictionary<string, EntityProperty>();
            dic.Add(nameof(BotId), new EntityProperty(BotId));
            dic.Add(nameof(UserOrChatId), new EntityProperty(UserOrChatId));
            dic.Add(nameof(ChatType), new EntityProperty((int)ChatType));
            dic.Add(nameof(Value), new EntityProperty(Value));
            dic.Add(nameof(Version), new EntityProperty(Version));

            dic.Add(EntityPropertyExtensions.SerializationVersionPropertyName, new EntityProperty(CurrentSerializationVersion));
            dic.Add(EntityPropertyExtensions.EntityTypePropertyName, new EntityProperty(nameof(BotUserOrChatContext)));
            return dic;
        }
    }
}
