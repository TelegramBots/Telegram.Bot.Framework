namespace NetTelegramBot.Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using NetTelegramBotApi;
    using NetTelegramBotApi.Requests;
    using NetTelegramBotApi.Types;
    using Storage;

    public abstract class BotBase
    {
        private ILogger logger;

        private IStorageService storageService;

        private ICommandParser commandParser;

        private TelegramBot botApi;

        public BotBase(ILogger logger, IStorageService storageService, ICommandParser commandParser, string token)
        {
            this.logger = logger;
            this.storageService = storageService;
            this.commandParser = commandParser;
            this.botApi = new TelegramBot(token);

            RefreshSelf().Wait();
            OnStart();
        }

        /// <summary>
        /// Telegram Id for this Bot
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Telegram Username for this Bot
        /// </summary>
        public string Username { get; private set; }

        public long LastOffset { get; private set; }

        public async Task RefreshSelf()
        {
            var me = await SendAsync(new GetMe());
            if (me == null)
            {
                throw new Exception("Can't get bot user info. Check API Token");
            }

            Id = me.Id;
            Username = me.Username;

            logger.LogInformation($"Bot info refreshed: {Username} (id = {Id})");
        }

        public virtual void OnStart()
        {
            // Nothing
        }

        public abstract Task OnCommand(Message message, BotCommand command);

        public abstract Task OnMessage(Message message);

        public async Task ProcessAsync(Update update)
        {
            var msg = update.Message;
            if (msg != null)
            {
                var saveMessageTask = storageService.SaveMessageAsync(msg);

                var command = commandParser.TryParse(msg.Text);
                if (command != null)
                {
                    await OnCommand(msg, command);
                }
                else
                {
                    await OnMessage(msg);
                }

                await Task.WhenAll(saveMessageTask);
            }
            LastOffset = update.UpdateId;
        }

        public Task<T> SendAsync<T>(RequestBase<T> message)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Sending {message.GetType().Name}...");
            }

            return botApi.MakeRequestAsync(message);
        }

        public async Task ProcessIncomingWebhookAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var text = await reader.ReadToEndAsync();
                var update = botApi.DeserializeUpdate(text);
                await ProcessAsync(update);
            }
        }

        public Task SaveContextAsync<TContext>(User user, TContext value)
            where TContext : class, new()
        {
            return storageService.SaveContextAsync(Id, user, value);
        }

        public Task SaveContextAsync<TContext>(Chat chat, TContext value)
            where TContext : class, new()
        {
            return storageService.SaveContextAsync(Id, chat, value);
        }

        public Task<TContext> LoadContextAsync<TContext>(long userOrChatId)
            where TContext : class, new()
        {
            return storageService.LoadContextAsync<TContext>(Id, userOrChatId);
        }

        public Task<Tuple<List<TContext>, ISegmentedQueryContinuationToken>>
            LoadAllContextsAsync<TContext>(ISegmentedQueryContinuationToken token = null)
            where TContext : class, new()
        {
            return storageService.LoadAllContextsAsync<TContext>(Id, token);
        }

        public Task<Tuple<List<TContext>, ISegmentedQueryContinuationToken>>
            LoadAllContextsAsync<TContext>(ChatType chatType, ISegmentedQueryContinuationToken token = null)
            where TContext : class, new()
        {
            return storageService.LoadAllContextsAsync<TContext>(Id, chatType, token);
        }
    }
}
