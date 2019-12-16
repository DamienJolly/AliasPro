using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomMuteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomMuteMessageEvent;

        private readonly IRoomController _roomController;

        public RoomMuteEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            if (room.OwnerId != session.Player.Id) return;

            room.Muted = !room.Muted;
            await room.SendAsync(new RoomMutedComposer(room.Muted));
        }
    }
}
