// ReSharper disable UnusedTypeParameter

using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IUpdatePollingManager<TBot>
        where TBot : IBot
    {
        Task RunAsync(
            GetUpdatesRequest requestParams = default,
            CancellationToken cancellationToken = default
        );
    }
}