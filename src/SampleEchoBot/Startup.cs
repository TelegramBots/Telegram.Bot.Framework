using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using SampleEchoBot.Commands;
using SampleEchoBot.Services;

namespace SampleEchoBot
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("SampleEchoBot_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var botOptions = new BotOptions<IEchoBot>
            {
                ApiToken = _configuration["EchoBot:ApiToken"],
                BotName = _configuration["EchoBot:BotName"],
                UseWebhook = _configuration.GetValue<bool?>("EchoBot:UseWebhook"),
                WebhookUrl = _configuration["EchoBot:WebhookUrl"],
            };

            services.AddTelegramBot<IEchoBot, EchoBot>()
                .AddBotOptions(botOptions)
                .AddDefaultMessageParser()
                .AddMessageHandler<IPhotoForwarder, PhotoForwarder>()
                .AddMessageHandler<ITextMessageEchoer, TextMessageEchoer>()
                .AddMessageHandler<IStartCommand, StartCommand>()
                .AddDefaultUpdateManager();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            Task.Factory.StartNew(async () =>
               {
                   while (true)
                   {
                       using (var scope = app.ApplicationServices.CreateScope())
                       {
                           //var opts = scope.ServiceProvider.GetRequiredService<IBotOptions<IEchoBot>>();
                           //var accessor = scope.ServiceProvider
                           //    .GetRequiredService<IMessageHandlersAccessor<IEchoBot>>();
                           //var parser = scope.ServiceProvider.GetRequiredService<IMessageParser<IEchoBot>>();
                           //var bot = scope.ServiceProvider.GetRequiredService<IEchoBot>();
                           var mgr = scope.ServiceProvider.GetRequiredService<IBotUpdateManager<IEchoBot>>();
                           mgr.Run(null);
                       }
                       await Task.Delay(2000);
                   }
               })
            .ContinueWith(t =>
            {
                Debug.WriteLine(t.Exception.InnerExceptions.First().Message);
            }, TaskContinuationOptions.OnlyOnFaulted)
            .ConfigureAwait(false);

        }
    }
}
