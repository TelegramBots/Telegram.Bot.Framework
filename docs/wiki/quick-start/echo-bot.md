# Echo Bot 🤖

This guide shows you how to quickly run your first bot using Visual Studio 2017 and ASP.NET Core.

**Complete code is in [SampleEchoBot project](../../../sample/SampleEchoBot)**.

This guide assumes that:

- You already have registered a bot and have its API Token.

## Project Setup

Create a new ASP.NET Core (empty) version 1.1 or above app and add `Telegram.Bot.Framework` NuGet package to it.

## Code

### Bot

Create your bot class:

```c#
public class EchoBot : BotBase<EchoBot> {
    public EchoBot(IOptions<BotOptions<EchoBot>> botOptions)
        : base(botOptions) { }

    public override Task HandleUnknownMessage(Update update) => Task.CompletedTask;

    public override Task HandleFaultedUpdate(Update update, Exception e) => Task.CompletedTask;
}
```

### Update Handlers

Create an `/echo` command that echoes user input back:

```c#
public class EchoCommandArgs : ICommandArgs {
    public string RawInput { get; set; }
    public string ArgsInput { get; set; }
}
public class EchoCommand : CommandBase<EchoCommandArgs> {
    public EchoCommand() : base(name: "echo") {}

    public override async Task<UpdateHandlingResult> HandleCommand(Update update, EchoCommandArgs args) {
        string replyText = string.IsNullOrWhiteSpace(args.ArgsInput) ? "Echo What?" : args.ArgsInput;

        await Bot.Client.SendTextMessageAsync(
            update.Message.Chat.Id,
            replyText,
            replyToMessageId: update.Message.MessageId);

        return UpdateHandlingResult.Handled;
    }
}
```

> Pros only: Add a `/start` command as well that replies with text "Hello World!".

### Startup

In `Startup` class, add the following code. This Adds Telegram bot and its update handlers to the DI
container and also uses long-polling method to get new updates every 3 seconds. If you are running this
as a console app, pressing Enter key will stop bot manager from getting updates.

```c#
public void ConfigureServices(IServiceCollection services) {
    services.AddTelegramBot<EchoBot>(_configuration.GetSection("EchoBot"))
        .AddUpdateHandler<EchoCommand>()
        .Configure();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
    var source = new CancellationTokenSource();
    Task.Factory.StartNew(() => {
        Console.WriteLine("Press Enter to stop bot manager...");
        Console.ReadLine();
        source.Cancel();
    });
    Task.Factory.StartNew(async () => {
        var botManager = app.ApplicationServices.GetRequiredService<IBotManager<EchoBot>>();
        while (!source.IsCancellationRequested) {
            await Task.Delay(3_000);
            await botManager.GetAndHandleNewUpdatesAsync();
        }
        Console.WriteLine("Bot manager stopped.");
    }).ContinueWith(t => {
        if (t.IsFaulted) throw t.Exception;
    });
}
```

> Pros only: Add the `/start` command you created to the list of handlers as well.

## Configurations

Add the following configurations to your `appsettings.json`.

```json
{
  "EchoBot": {
    "ApiToken": "{your-bots-api-token}",
    "BotUserName": "{your-bots-username}"
  }
}
```

## Run

That's it! Run your the app and write to your bot: `/echo Hello`
