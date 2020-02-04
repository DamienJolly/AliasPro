using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestFloorPlanDoorSettingsEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestFloorPlanDoorSettingsMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            if (session.CurrentRoom == null) 
                return;

            await session.SendPacketAsync(new FloorPlanDoorSettingsComposer(session.CurrentRoom));
            //await session.SendPacketAsync(new RoomThicknessComposer(session.Player.CurrentRoom));
        }
    }
}
