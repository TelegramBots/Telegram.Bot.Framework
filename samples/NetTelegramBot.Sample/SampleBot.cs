namespace NetTelegramBot.Sample
{
    using System;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Framework.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NetTelegramBot.Framework;
    using NetTelegramBotApi.Requests;
    using NetTelegramBotApi.Types;

    public class SampleBot : BotBase
    {
        private readonly SampleBotOptions options;

        private readonly IStorageService storageService;

        public SampleBot(
            ILogger<SampleBot> logger,
            IStorageService storageService,
            ICommandParser commandParser,
            IOptions<SampleBotOptions> options)
            : base(logger, storageService, commandParser, options.Value.Token)
        {
            this.options = options.Value;
            this.storageService = storageService;

            this.CommandHandlers.Add("sendall", new SendAll(storageService));
        }

        public override Task OnUnknownCommand(Message message, ICommand command)
        {
            return SendAsync(new SendMessage(message.Chat.Id, "Unknown command :(")
            {
                ReplyToMessageId = message.MessageId
            });
        }

        public override async Task OnMessage(Message message)
        {
            // This is "regular" chat message
            // Do nothing with message itself, but save ChatId to be able to /sendall here later
            var chatContext = await storageService.LoadContextAsync<SampleUserContext>(Id, message.Chat.Id);

            // It's our first message in this chat. Create context and save
            if (chatContext == null)
            {
                chatContext = new SampleUserContext
                {
                    FirstContact = DateTimeOffset.Now,
                    ChatId = message.Chat.Id,
                    IsChat = message.Chat.GetChatType() != ChatType.Private
                };
                await storageService.SaveContextAsync(Id, message.Chat, chatContext);
            }

            // Do something with message :)
            var from = message.From;
            var text = message.Text;
            var photos = message.Photo;
            var contact = message.Contact;
            var location = message.Location;
            Console.WriteLine(
                "Msg from {0} {1} ({2}) at {4}: {3}",
                from.FirstName,
                from.LastName,
                from.Username,
                text,
                message.Date);
        }
    }
}
