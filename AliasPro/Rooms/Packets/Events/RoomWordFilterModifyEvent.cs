using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomWordFilterModifyEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomWordFilterModifyMessageEvent;

        private readonly IRoomController _roomController;

        public RoomWordFilterModifyEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            if (!_roomController.TryGetRoom(roomId, out IRoom room))
                return;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            bool add = clientPacket.ReadBool();
            string word = clientPacket.ReadString();

            if (word.Length > 25)
                word = word.Substring(0, 24);

            if (add)
                await _roomController.AddRoomWordFilter(word, room);
            else
                await _roomController.RemoveRoomWordFilter(word, room);
        }
    }
}
