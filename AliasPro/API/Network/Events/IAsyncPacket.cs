using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;

namespace AliasPro.API.Network.Events
{
    public interface IAsyncPacket
    {
        short Header { get; }
        void HandleAsync(ISession session, IClientPacket clientPacket);
    }
}
