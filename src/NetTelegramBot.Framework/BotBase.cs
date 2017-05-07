using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class BotBase<TBot> : IBot
        where TBot : BotBase<TBot>
    {
        //private readonly ILogger _logger;

        //private IStorageService storageService;

        protected readonly TelegramBot Bot;

        protected readonly IMessageParser<TBot> MessageParser;

        protected BotBase(IBotOptions<TBot> botOptions,
            IMessageParser<TBot> messageParser
            //,ILogger logger
            //,IStorageService storageService
            )
        {
            Bot = new TelegramBot(botOptions.ApiToken);
            MessageParser = messageParser;
            //_logger = logger;
            //this.storageService = storageService;
            //this.commandParser = commandParser;

            //OnStart();
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

        public virtual Task ProcessAsync(Update update)
        {
            throw new NotImplementedException();
            /*
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
                _logger.LogError(0, ex, "SendAsync-related error during message processing. Ignored.");
            }
            LastOffset = update.UpdateId;
            */
        }

        public virtual Task<T> SendAsync<T>(RequestBase<T> message)
        {
            throw new NotImplementedException();
            /*
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Sending {message.GetType().Name}...");
            }

            return _bot.MakeRequestAsync(message);
            */
        }

        public virtual Task ProcessIncomingWebhookAsync(Stream stream)
        {
            throw new NotImplementedException();
            /*
            using (var reader = new StreamReader(stream))
            {
                var text = await reader.ReadToEndAsync();
                var update = _bot.DeserializeUpdate(text);
                await ProcessAsync(update);
            }
            */
        }

        public virtual Task OnCommand(Message message, ICommand command)
        {
            throw new NotImplementedException();
            /*
            ICommandHandler handler;
            if (CommandHandlers.TryGetValue(command.Name, out handler))
            {
                return handler.Execute(command, this, message);
            }
            else
            {
                return OnUnknownCommand(message, command);
            }
            */
        }

        public abstract Task HandleUnknownMessageAsync(Message message);

        /// <summary>
        /// Sends 'getMe' request, fills <see cref="Id"/> and <see cref="Username"/> from response
        /// </summary>
        protected virtual void OnStart()
        {
            throw new NotImplementedException();
            /*
            var me = SendAsync(new GetMe()).Result;
            if (me == null)
            {
                throw new Exception("Can't get bot user info. Check API Token");
            }

            Id = me.Id;
            Username = me.Username;

            _logger.LogInformation($"Bot info refreshed: {Username} (id = {Id})");
            */
        }

        public User BotUserInfo { get; }

        public async Task<T> MakeRequestAsync<T>(RequestBase<T> request)
        {
            return await Bot.MakeRequestAsync(request)
                .ConfigureAwait(false);
        }

        public virtual async Task ProcessUpdateAsync(Update update)
        {
            //if (update?.Message != null)
            {
                var handlers = MessageParser.FindMessageHandlers(update).ToArray();
                if (handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        handler.Bot = this;
                        await handler.HandleMessageAsync(update);
                    }
                }
                else
                {
                    await HandleUnknownMessageAsync(update.Message);
                }
            }
        }
    }
}
