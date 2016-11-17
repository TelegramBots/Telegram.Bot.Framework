namespace NetTelegramBot.Sample
{
    using System;
    using System.Threading.Tasks;
    using Framework.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NetTelegramBot.Framework;
    using NetTelegramBotApi.Requests;
    using NetTelegramBotApi.Types;

    public class SampleBot : BotBase
    {
        private ILogger logger;

        private SampleBotOptions options;

        public SampleBot(ILogger<SampleBot> logger, IStorageService storageService, ICommandParser commandParser, IOptions<SampleBotOptions> options)
            : base(logger, storageService, commandParser, options.Value.Token)
        {
            this.options = options.Value;
        }

        public override async Task OnCommand(Message message, BotCommand command)
        {
            if (command.Command == "SENDALL")
            {
                if ((message.From.Username?.Length ?? 0) < 5) // some "security" :)
                {
                    await SendAsync(new SendMessage(message.Chat.Id, "Forbidden. Sorry.")
                    {
                        ReplyToMessageId = message.MessageId
                    });

                    return;
                }

                await SendAsync(new SendMessage(message.Chat.Id, "Accepted")
                {
                    ReplyToMessageId = message.MessageId
                });

                // join all parts back (alternatively, re-parse message.Text)
                var textToSend = string.Join(" ", command.Args);

                var list = await LoadAllContextsAsync<SampleUserContext>(null);
                while (list.Item1.Count != 0)
                {
                    var tasks = new Task[list.Item1.Count];
                    for (var i = 0; i < list.Item1.Count; i++)
                    {
                        tasks[i] = SendAsync(new SendMessage(list.Item1[i].ChatId, textToSend));
                    }

                    await Task.WhenAll(tasks);

                    if (list.Item2 == null)
                    {
                        break;
                    }

                    list = await LoadAllContextsAsync<SampleUserContext>(list.Item2);
                }

            }
        }

        public override async Task OnMessage(Message message)
        {
            // We want to store all ChatId and send them message later
            var chatContext = await LoadContextAsync<SampleUserContext>(message.Chat.Id);
            if (chatContext == null)
            {
                chatContext = new SampleUserContext
                {
                    FirstContact = DateTimeOffset.Now,
                    ChatId = message.Chat.Id,
                    IsChat = message.Chat.GetChatType() != ChatType.Private
                };
                await SaveContextAsync(message.Chat, chatContext);
            }

            // Do something :)
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
