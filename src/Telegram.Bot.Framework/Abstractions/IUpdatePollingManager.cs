using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstractions
{
    public interface IUpdatePollingManager<TBot>
        where TBot : IBot
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}