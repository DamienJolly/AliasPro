using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserUnbanEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserUnbanMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserUnbanEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            int roomId = clientPacket.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            room.Bans.UnbanPlayer(playerId);
            await _roomController.RemoveRoomBan(room.Id, playerId);
        }
    }
}
