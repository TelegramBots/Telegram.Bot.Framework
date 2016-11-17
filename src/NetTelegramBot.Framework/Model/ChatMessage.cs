namespace NetTelegramBot.Framework.Model
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using NetTelegramBotApi.Types;

    public class ChatMessage : TableEntity
    {
        private const int CurrentSerializationVersion = 1;

        [Obsolete("This constructor is for Azure infrastructure only!")]
        public ChatMessage()
        {
            // Nothing
        }

        public ChatMessage(Message message)
        {
            this.Message = message;
            this.ChatId = message.Chat.Id;
            this.FromId = message.From.Id;
            this.HasText = !string.IsNullOrEmpty(message.Text);

            var key = GetKey(message.Chat.Id, message.Date, message.MessageId);
            PartitionKey = key.PartitionKey;
            RowKey = key.RowKey;
        }

        public Message Message { get; set; }

        public long ChatId { get; set; }

        public long FromId { get; set; }

        public bool HasText { get; set; }

        public static EntityKey<ChatMessage> GetKey(long chatId, DateTimeOffset timestamp, long messageId)
        {
            return new EntityKey<ChatMessage>
            {
                PartitionKey = chatId.ToString(),
                RowKey = timestamp.GetInvertedTicks() + "_" + messageId
            };
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            //// base.ReadEntity(properties, operationContext); - there is no any 'base' properties

            var sourceEntityType = properties[EntityPropertyExtensions.EntityTypePropertyName].StringValue;
            if (!string.Equals(nameof(ChatMessage), sourceEntityType))
            {
                throw new Exception($"Can't restore from entity type {sourceEntityType}");
            }

            var serializationVersion = properties[EntityPropertyExtensions.SerializationVersionPropertyName].Int32Value.Value;
            if (serializationVersion > CurrentSerializationVersion)
            {
                throw new Exception($"Can't restore from serialization version {serializationVersion}");
            }

            Message = properties.Deserialize<Message>(nameof(Message));
            ChatId = properties[nameof(ChatId)].Int64Value.Value;
            FromId = properties[nameof(FromId)].Int64Value.Value;
            HasText = properties[nameof(HasText)].BooleanValue.Value;
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            //// var dic = base.WriteEntity(operationContext); - there is no any 'base' properties

            var dic = new Dictionary<string, EntityProperty>();
            dic.AddSerialized(nameof(Message), Message);
            dic.Add(nameof(ChatId), new EntityProperty(ChatId));
            dic.Add(nameof(FromId), new EntityProperty(FromId));
            dic.Add(nameof(HasText), new EntityProperty(HasText));

            dic.Add(EntityPropertyExtensions.SerializationVersionPropertyName, new EntityProperty(CurrentSerializationVersion));
            dic.Add(EntityPropertyExtensions.EntityTypePropertyName, new EntityProperty(nameof(ChatMessage)));
            return dic;
        }
    }
}
