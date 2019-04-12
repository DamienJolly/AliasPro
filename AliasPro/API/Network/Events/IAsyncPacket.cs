using AliasPro.API.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.API.Network.Events
{
    public interface IAsyncPacket
    {
        short Header { get; }
        void HandleAsync(ISession session, IClientPacket clientPacket);
    }
}
