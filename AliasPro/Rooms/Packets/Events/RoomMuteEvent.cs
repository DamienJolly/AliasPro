using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomMuteEvent : IMessageEvent
    {
        public short Header => Incoming.RoomMuteMessageEvent;

        private readonly IRoomController _roomController;

        public RoomMuteEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

            if (room.OwnerId != session.Player.Id) return;

            room.Muted = !room.Muted;
            await room.SendPacketAsync(new RoomMutedComposer(room.Muted));
        }
    }
}
