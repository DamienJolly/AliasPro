using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserUnbanEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserUnbanMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserUnbanEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();
            int roomId = message.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) 
                return;

            room.Bans.UnbanPlayer(playerId);
            await _roomController.RemoveRoomBan(room.Id, playerId);
        }
    }
}
