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
            IBotUpdateManager<EchoBot> botManager = UseSimpleInjector("API_TOKEN_HERE");

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
            var botServiceContainer = new BotServiceContainer<EchoBot>(container);

            IBotUpdateManager<EchoBot> botManager = botServiceContainer
                .Bot(() => new EchoBot(apiToken))
                .Use<ExceptionHandler>()
                .Use<TextMessageHandler>()
                .Register();

            return botManager;
        }
    }
}