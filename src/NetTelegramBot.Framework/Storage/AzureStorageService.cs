namespace NetTelegramBot.Framework.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Model;
    using NetTelegramBotApi.Types;
    using Newtonsoft.Json;

    public class AzureStorageService : IStorageService
    {
        private readonly AzureStorageServiceOptions options;

        private readonly ILogger logger;

        private readonly List<CloudTable> knownTables = new List<CloudTable>();

        public AzureStorageService(
            IOptions<AzureStorageServiceOptions> options,
            ILogger<AzureStorageService> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// Saves message. Previous one (with same MessageId) will be overwritten.
        /// </summary>
        public async Task SaveMessageAsync(Message message)
        {
            var tableName = options.LogTablePrefix + message.Date.UtcDateTime.Year;
            var table = await GetTable(tableName);

            var entity = new ChatMessage(message);
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async Task SaveContextAsync<TContext>(long botId, User user, TContext userContext)
            where TContext : class, new()
        {
            var value = JsonConvert.SerializeObject(userContext, EntityPropertyExtensions.JsonSettings);
            var entity = new BotUserOrChatContext(botId, user.Id, ChatType.Private, value);

            var table = await GetTable(options.MainTableName);
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async Task SaveContextAsync<TContext>(long botId, Chat chat, TContext userContext)
            where TContext : class, new()
        {
            var value = JsonConvert.SerializeObject(userContext, EntityPropertyExtensions.JsonSettings);
            var entity = new BotUserOrChatContext(botId, chat.Id, chat.GetChatType(), value);

            var table = await GetTable(options.MainTableName);
            await table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async Task<TContext> LoadContextAsync<TContext>(long botId, long userOrChatId)
            where TContext : class, new()
        {
            var table = await GetTable(options.MainTableName);

            var key = BotUserOrChatContext.GetKey(botId, userOrChatId);
            var obj = await table.ExecuteAsync(TableOperation.Retrieve<BotUserOrChatContext>(key.PartitionKey, key.RowKey));

            if (obj.Result == null)
            {
                return null;
            }

            var context = (BotUserOrChatContext)obj.Result;

            if (string.IsNullOrEmpty(context.Value))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<TContext>(context.Value, EntityPropertyExtensions.JsonSettings);
        }

        public async Task<Tuple<List<TContext>, ISegmentedQueryContinuationToken>>
            LoadAllContextsAsync<TContext>(long botId, ISegmentedQueryContinuationToken token = null)
            where TContext : class, new()
        {
            var continuationToken = (AzureStorageServiceContinuationToken)token;

            var table = await GetTable(options.MainTableName);

            var query = BotUserOrChatContext.GetAllForBot(botId);

            var segment = await table.ExecuteQuerySegmentedAsync(query, continuationToken?.Token);

            var contexts = segment.Results.Select(
                x => string.IsNullOrEmpty(x.Value)
                    ? null
                    : JsonConvert.DeserializeObject<TContext>(x.Value, EntityPropertyExtensions.JsonSettings))
                    .ToList();

            var newToken =
                segment.ContinuationToken == null
                ? null
                : new AzureStorageServiceContinuationToken { Token = segment.ContinuationToken };
            return Tuple.Create(contexts, (ISegmentedQueryContinuationToken)newToken);
        }

        public async Task<Tuple<List<TContext>, ISegmentedQueryContinuationToken>>
            LoadAllContextsAsync<TContext>(long botId, ChatType chatType, ISegmentedQueryContinuationToken token = null)
            where TContext : class, new()
        {
            var continuationToken = (AzureStorageServiceContinuationToken)token;

            var table = await GetTable(options.MainTableName);

            var query = BotUserOrChatContext.GetAllForBot(botId, chatType);

            var segment = await table.ExecuteQuerySegmentedAsync(query, continuationToken?.Token);

            var contexts = segment.Results.Select(
                x => string.IsNullOrEmpty(x.Value)
                    ? null
                    : JsonConvert.DeserializeObject<TContext>(x.Value, EntityPropertyExtensions.JsonSettings))
                    .ToList();

            var newToken =
                segment.ContinuationToken == null
                ? null
                : new AzureStorageServiceContinuationToken { Token = segment.ContinuationToken };
            return Tuple.Create(contexts, (ISegmentedQueryContinuationToken)newToken);
        }

        protected async Task<CloudTable> GetTable(string tableName)
        {
            var table = knownTables.FirstOrDefault(x => string.Equals(x.Name, tableName, StringComparison.OrdinalIgnoreCase));
            if (table != null)
            {
                return table;
            }

            var account = CloudStorageAccount.Parse(options.ConnectionString);
            var cloudTableClient = account.CreateCloudTableClient();

            table = cloudTableClient.GetTableReference(tableName);
            var isTableCreated = await table.CreateIfNotExistsAsync();
            if (isTableCreated)
            {
                logger.LogInformation($"Table created: {tableName}");
            }

            lock (knownTables)
            {
                knownTables.Add(table);
            }
            return table;
        }

        public class AzureStorageServiceContinuationToken : ISegmentedQueryContinuationToken
        {
            public TableContinuationToken Token { get; set; }
        }
    }
}
