using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Abstractions
{
    public interface IBotUpdateManager<TBot>
        where TBot : IBot
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}