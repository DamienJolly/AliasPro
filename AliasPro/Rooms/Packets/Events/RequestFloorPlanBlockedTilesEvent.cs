using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestFloorPlanBlockedTilesEvent : IMessageEvent
    {
        public short Header => Incoming.RequestFloorPlanBlockedTilesMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            if (session.CurrentRoom == null) return;

            await session.SendPacketAsync(new FloorPlanBlockedTilesComposer(session.CurrentRoom.RoomGrid.GetLockedTiles()));
        }
    }
}
