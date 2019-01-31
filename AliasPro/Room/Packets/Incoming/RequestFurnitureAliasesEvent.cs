using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    public class RequestFurnitureAliasesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFurnitureAliasesMessageEvent;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.WriteAndFlushAsync(new FurnitureAliasesComposer());
        }
    }
}
