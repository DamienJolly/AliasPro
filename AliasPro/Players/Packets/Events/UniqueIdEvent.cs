using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Players.Packets.Events
{
    public class UniqueIdEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UniqueIdMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadString();
            session.UniqueId = clientPacket.ReadString();
        }
    }
}
