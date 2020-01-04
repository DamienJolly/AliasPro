using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestRoomRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestRoomRightsMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;

            if (room == null) return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            await session.SendPacketAsync(new RoomRightsListComposer((int)room.Id, room.Rights.Rights));
        }
    }
}
