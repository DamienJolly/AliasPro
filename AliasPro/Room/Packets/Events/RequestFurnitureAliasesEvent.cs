using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
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
