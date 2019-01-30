using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Network.Events
{
    public interface IAsyncPacket
    {
        short Header { get; }
        Task HandleAsync(ISession session, IClientPacket clientPacket);
    }
}
