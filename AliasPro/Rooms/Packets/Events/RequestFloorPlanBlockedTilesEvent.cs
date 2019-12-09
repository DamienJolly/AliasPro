using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestFloorPlanBlockedTilesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFloorPlanBlockedTilesMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.CurrentRoom == null) return;

            await session.SendPacketAsync(new FloorPlanBlockedTilesComposer(session.CurrentRoom.RoomGrid.GetLockedTiles()));
        }
    }
}
