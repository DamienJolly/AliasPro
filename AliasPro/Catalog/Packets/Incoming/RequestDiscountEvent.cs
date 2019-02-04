using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestDiscountEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestDiscountMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new DiscountComposer());
        }
    }
}
