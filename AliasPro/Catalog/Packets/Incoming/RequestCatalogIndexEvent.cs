using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestCatalogIndexEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestCatalogIndexMessageEvent;

        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: not used??
            return Task.CompletedTask;
        }
    }
}
