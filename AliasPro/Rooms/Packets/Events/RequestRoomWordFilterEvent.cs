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
    public class RequestRoomWordFilterEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestRoomWordFilterMessageEvent;

        private readonly IRoomController _roomController;

        public RequestRoomWordFilterEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }
        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            if (!_roomController.TryGetRoom(roomId, out IRoom room))
                return;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            await session.SendPacketAsync(new RoomFilterWordsComposer(room.WordFilter));
        }
    }
}
