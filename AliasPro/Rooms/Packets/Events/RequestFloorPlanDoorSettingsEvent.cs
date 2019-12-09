using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestFloorPlanDoorSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFloorPlanDoorSettingsMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (session.CurrentRoom == null) return;

            await session.SendPacketAsync(new FloorPlanDoorSettingsComposer(session.CurrentRoom));
            //await session.SendPacketAsync(new RoomThicknessComposer(session.Player.CurrentRoom));
        }
    }
}
