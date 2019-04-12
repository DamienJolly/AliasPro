using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Room.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class RequestFurnitureAliasesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFurnitureAliasesMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new FurnitureAliasesComposer());
        }
    }
}
