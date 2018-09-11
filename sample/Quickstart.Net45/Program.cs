using Quickstart.Net45.Handlers;
using Quickstart.Net45.Handlers.Commands;
using SimpleInjector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Abstractions;

namespace Quickstart.Net45
{
    class Program
    {
        public static void Main(string[] args)
        {
            string token = Environment.GetEnvironmentVariable("BOT_API_TOKEN") ?? "YOUR_API_TOKEN_HERE";
            IBotUpdateManager<EchoBot> botManager = UseSimpleInjector(token);

            var tokenSrc = new CancellationTokenSource();
            Task.Run(() =>
            {
                Console.ReadLine();
                tokenSrc.Cancel();
            });

            botManager.RunAsync(tokenSrc.Token).GetAwaiter().GetResult();
        }

        static IBotUpdateManager<EchoBot> UseSimpleInjector(string apiToken)
        {
            var container = new Container();
            var botBuilder = new Services.SimpleInjector.BotBuilder<EchoBot>(container);

            IBotUpdateManager<EchoBot> botManager = botBuilder
                .Bot(() => new EchoBot(apiToken))
                .Use<ExceptionHandler>()
                .UseCommand<Ping>()
                .UseCommand<StartCommand>()
                .UseWhen<TextEchoer>((_, context) => context.Update.Message?.Text != null)
                .UseWhen<CallbackQueryHandler>((_, context) => context.Update.CallbackQuery != null)
                .Register();

            return botManager;
        }
    }
}