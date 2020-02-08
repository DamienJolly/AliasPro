using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomWordFilterModifyEvent : IMessageEvent
    {
        public short Header => Incoming.RoomWordFilterModifyMessageEvent;

        private readonly IRoomController _roomController;

        public RoomWordFilterModifyEvent(IRoomController roomController)
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

            if (!room.Rights.HasRights(session.Player.Id)) 
                return;

            bool add = clientPacket.ReadBoolean();
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
