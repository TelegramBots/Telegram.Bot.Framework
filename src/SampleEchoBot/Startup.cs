using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTelegramBot.Framework;
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
            var echoBotToken = _configuration["EchoBot:ApiToken"];
            var echoBotUseWebhook = _configuration.GetValue<bool?>("EchoBot:UseWebhook");
            var echoBotWebhook = _configuration["EchoBot:WebhookUrl"];
            services.AddTransient<IBotOptions<EchoBot>, EchoBotOptions>(factory => new EchoBotOptions
            {
                ApiToken = echoBotToken,
                UseWebhook = echoBotUseWebhook,
                WebhookUrl = echoBotWebhook,
            });


            services.AddScoped<IMessageForwarder, MessageForwarder>();
            services.AddScoped<IMessageHandlersAccessor<EchoBot>>(factory => new EchoBotMessageHandlerAccessor(new[]
            {
                (IMessageHandler<EchoBot>)factory.GetRequiredService<IMessageForwarder>(),
            }));

            services.AddScoped<IMessageParser<EchoBot>, EchoBotMessageParser>();
            services.AddScoped<IEchoBot, EchoBot>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
