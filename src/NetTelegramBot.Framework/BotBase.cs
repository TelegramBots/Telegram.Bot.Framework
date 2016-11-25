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

            CommandHandlers = new Dictionary<string, ICommandHandler>(StringComparer.OrdinalIgnoreCase);

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

        public Dictionary<string, ICommandHandler> CommandHandlers { get; private set; }

        public virtual async Task ProcessAsync(Update update)
        {
            try
            {
                var msg = update.Message;
                if (msg != null)
                {
                    await storageService.SaveMessageAsync(msg);

                    var command = commandParser.TryParse(msg.Text);
                    if (command != null)
                    {
                        await OnCommand(msg, command);
                    }
                    else
                    {
                        await OnMessage(msg);
                    }
                }
            }
            catch (BotRequestException ex)
            {
                // Catch BotRequestException and ignore it.
                // Otherwise, incoming mesage will be processed again and again...
                // To avoid - just catch exception yourself (inside OnCommand/OnMessage),
                //     put inside other (AggregateException for example) and re-throw
                logger.LogError(0, ex, "SendAsync-related error during message processing. Ignored.");
            }
            LastOffset = update.UpdateId;
        }

        public virtual Task<T> SendAsync<T>(RequestBase<T> message)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug($"Sending {message.GetType().Name}...");
            }

            return botApi.MakeRequestAsync(message);
        }

        public virtual async Task ProcessIncomingWebhookAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var text = await reader.ReadToEndAsync();
                var update = botApi.DeserializeUpdate(text);
                await ProcessAsync(update);
            }
        }

        public virtual Task OnCommand(Message message, ICommand command)
        {
            ICommandHandler handler;
            if (CommandHandlers.TryGetValue(command.Name, out handler))
            {
                return handler.Execute(command, this, message);
            }
            else
            {
                return OnUnknownCommand(message, command);
            }
        }

        public abstract Task OnUnknownCommand(Message message, ICommand command);

        public abstract Task OnMessage(Message message);

        /// <summary>
        /// Sends 'getMe' request, fills <see cref="Id"/> and <see cref="Username"/> from response
        /// </summary>
        protected virtual void OnStart()
        {
            var me = SendAsync(new GetMe()).Result;
            if (me == null)
            {
                throw new Exception("Can't get bot user info. Check API Token");
            }

            Id = me.Id;
            Username = me.Username;

            logger.LogInformation($"Bot info refreshed: {Username} (id = {Id})");
        }
    }
}
