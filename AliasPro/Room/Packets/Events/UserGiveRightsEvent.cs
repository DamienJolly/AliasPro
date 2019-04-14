using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserGiveRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserGiveRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public UserGiveRightsEvent(IPlayerController playerController, IRoomController roomController)
        {
            _playerController = playerController;
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            int entityId = clientPacket.ReadInt();

            if (!room.Entities.TryGetEntityById(entityId, out BaseEntity entity)) return;

            if (entity is PlayerEntity userEntity)
            {
                if (room.Rights.HasRights(userEntity.Player.Id)) return;
                
                room.Rights.GiveRights(userEntity.Player.Id, userEntity.Player.Username);
                await _roomController.GiveRoomRights(room.Id, userEntity.Player.Id);
                await room.Rights.ReloadRights(userEntity.Session);
            }
        }
    }
}

