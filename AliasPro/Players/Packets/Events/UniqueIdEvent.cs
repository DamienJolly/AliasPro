using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;

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
            //todo: await send composer??
        }
    }
}
