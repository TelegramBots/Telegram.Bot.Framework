using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    class UpdateMembersList : IUpdateHandler
    {
        private readonly ILogger<UpdateMembersList> _logger;

        public UpdateMembersList(ILogger<UpdateMembersList> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "There were updates to the members list of chat {0}.",
                context.Update.Message?.Chat.Id ?? context.Update.ChannelPost.Chat.Id
            );

            return next(context, cancellationToken);
        }
    }
}