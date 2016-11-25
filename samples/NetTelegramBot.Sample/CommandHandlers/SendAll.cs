namespace NetTelegramBot.Sample.CommandHandlers
{
    using System;
    using System.Threading.Tasks;
    using Framework.Storage;
    using NetTelegramBot.Framework;
    using NetTelegramBotApi;
    using NetTelegramBotApi.Requests;
    using NetTelegramBotApi.Types;

    /// <summary>
    /// This command sends text to all chats (users and groups) it was added to.
    /// </summary>
    public class SendAll : ICommandHandler
    {
        private readonly IStorageService storageService;

        public SendAll(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public async Task Execute(ICommand command, BotBase bot, Message message)
        {
            if ((message.From.Username?.Length ?? 0) < 5) // some "security" :)
            {
                await bot.SendAsync(new SendMessage(message.Chat.Id, "Forbidden. Sorry.")
                {
                    ReplyToMessageId = message.MessageId
                });

                return;
            }

            await bot.SendAsync(new SendMessage(message.Chat.Id, "Accepted")
            {
                ReplyToMessageId = message.MessageId
            });

            // join all parts back (alternatively, you may re-parse message.Text)
            var textToSend = string.Join(" ", command.Args);

            var list = await storageService.LoadAllContextsAsync<SampleUserContext>(bot.Id);
            while (list.Item1.Count != 0)
            {
                var tasks = new Task[list.Item1.Count];
                for (var i = 0; i < list.Item1.Count; i++)
                {
                    tasks[i] = bot.SendAsync(new SendMessage(list.Item1[i].ChatId, textToSend));
                }

                await Task.WhenAll(tasks);

                if (list.Item2 == null)
                {
                    break;
                }

                list = await storageService.LoadAllContextsAsync<SampleUserContext>(bot.Id, list.Item2);
            }
        }
    }
}
