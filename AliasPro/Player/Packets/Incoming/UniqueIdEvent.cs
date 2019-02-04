using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    public class UniqueIdEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UniqueIdMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadString();
            string uniqueId = clientPacket.ReadString();
            session.UniqueId = uniqueId;
            //todo: await send composer
        }
    }
}
