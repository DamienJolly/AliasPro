using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserMuteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserMuteMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserMuteEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            int roomId = clientPacket.ReadInt();
            int minutes = clientPacket.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            switch (room.Settings.WhoMutes)
            {
                case 0: default: if (!room.Rights.IsOwner(session.Player.Id)) return; break;
                case 1: if (!room.Rights.HasRights(session.Player.Id)) return; break;
            }

            if (room.Rights.HasRights((uint)playerId)) return;

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
                return;

            room.Mute.MutePlayer(playerId, (int)UnixTimestamp.Now + (minutes * 60));
        }
    }
}
