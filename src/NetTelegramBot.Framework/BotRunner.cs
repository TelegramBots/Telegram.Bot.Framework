namespace NetTelegramBot.Framework
{
    using System;
    using NetTelegramBotApi.Requests;
    using RecurrentTasks;

    public class BotRunner<T> : IRunnable
        where T : BotBase
    {
        private T bot;

        public BotRunner(T bot)
        {
            this.bot = bot;
        }

        public void Run(ITask currentTask)
        {
            while (true)
            {
                var updates = bot.SendAsync(new GetUpdates { Offset = bot.LastOffset + 1 }).Result;

                if (updates == null || updates.Length == 0)
                {
                    break;
                }

                foreach (var update in updates)
                {
                    bot.ProcessAsync(update).Wait();
                }
            }
        }
    }
}
