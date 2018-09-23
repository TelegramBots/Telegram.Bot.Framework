using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.Handlers;
using Quickstart.AspNetCore.Services;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotOptions<EchoBot>>(Configuration.GetSection("EchoBot"));
            services.AddTransient<EchoBot>();

            services
                .AddScoped<ExceptionHandler>()
                .AddScoped<WebhookLogger>()
                .AddScoped<CallbackQueryHandler>()
                .AddScoped<TextEchoer>()
                .AddScoped<PingCommand>()
                .AddScoped<StartCommand>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<UpdateMembersList>();

            services.AddScoped<IWeatherService, WeatherService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EchoBot>(ConfigureBot(), startAfter: TimeSpan.FromSeconds(1));
            }
            else
            {
                // use Telegram bot webhook middleware in higher environments
                app.UseTelegramBotWebhook<EchoBot>(ConfigureBot());
                // and make sure webhook is enabled
                app.EnsureWebhookSet<EchoBot>(baseUrl: "https://quickstart-tgbot.herokuapp.com");
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()

                // .Use<CustomUpdateLogger>()
                .UseWhen<WebhookLogger>(When.Webhook)

                .UseWhen<UpdateMembersList>(When.MembersChanged)

                .MapWhen(When.NewMessage, msgBranch => msgBranch
                    .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                        .Use<TextEchoer>()
                        .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<PingCommand>("ping")
                            .UseCommand<StartCommand>("start")
                        )
                    //.Use<NLP>()
                    )
                    .MapWhen(When.StickerMessage, branch => branch.Use<StickerHandler>())
                    .MapWhen(When.LocationMessage, branch => branch.Use<WeatherReporter>())
                )

                .MapWhen<CallbackQueryHandler>(When.CallbackQuery)

                // .Use<UnhandledUpdateReporter>()
                ;
        }
    }
}
