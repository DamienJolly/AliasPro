using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomRightsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestRoomRightsMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            await session.SendPacketAsync(new RoomRightsListComposer((int)room.Id, room.Rights.Rights));
        }
    }
}
