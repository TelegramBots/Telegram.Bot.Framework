using Quickstart.Net45.Handlers;
using Quickstart.Net45.Services;
using Quickstart.Net45.Services.SimpleInjector;
using SimpleInjector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace Quickstart.Net45
{
    class Program
    {
        public static void Main(string[] args)
        {
            string token = Environment.GetEnvironmentVariable("BOT_API_TOKEN") ?? "YOUR_API_TOKEN_HERE";

            var container = new Container();
            container.Register(() => new EchoBot(token));
            container.Register<ExceptionHandler>();
            container.Register<IWeatherService, WeatherService>();
            container.Verify();

            UpdateDelegate updateCallback = ConfigureBot(new BotBuilder<EchoBot>());
            var mgr = new BotUpdateManager<EchoBot>(updateCallback, new BotServiceProvider(container));

            var tokenSrc = new CancellationTokenSource();
            Task.Run(() =>
            {
                Console.ReadLine();
                tokenSrc.Cancel();
            });

            mgr.RunAsync(tokenSrc.Token).GetAwaiter().GetResult();
        }

        static UpdateDelegate ConfigureBot(IBotBuilder bot)
        {
            return bot
                .Use<ExceptionHandler>()
                .UseWhen(When.IsWebhook, branch => branch.Use<WebhookLogger>())
                .Map(UpdateType.CallbackQuery, branch => branch.Use<CallbackQueryHandler>())
                .UseWhen(When.NewTextMessage, branch => branch.Use<TextEchoer>())
                .UseCommand<PingCommand>("ping")
                .UseCommand<StartCommand>("start")
                .MapWhen(When.StickerMessage, branch => branch.Use<StickerHandler>())
                .MapWhen(When.LocationMessage, branch => branch.Use<WeatherReporter>())
                .UseWhen(When.MembersChanged, branch => branch.Use<UpdateMembersList>())
                .Build()
            ;
        }
    }
}