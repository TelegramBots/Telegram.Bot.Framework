using System.Threading.Tasks;

namespace Telegram.Bot.Abstractions
{
    public delegate Task UpdateDelegate(IUpdateContext context);
}