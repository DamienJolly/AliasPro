using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Network.Events.Headers;
using AliasPro.Players;
using AliasPro.Room.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Sessions;

namespace AliasPro.Room.Packets.Events
{
    public class UserRemoveRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserRemoveRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public UserRemoveRightsEvent(IPlayerController playerController, IRoomController roomController)
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

            if (!room.RightHandler.IsOwner(session.Player.Id)) return;

            int amount = clientPacket.ReadInt();

            for (int i = 0; i < amount; i++)
            {
                int entityId = clientPacket.ReadInt();

                if (!room.EntityHandler.TryGetEntityById(entityId, out BaseEntity entity)) return;

                if (entity is UserEntity userEntity)
                {
                    if (!room.RightHandler.HasRights(userEntity.Player.Id)) return;

                    room.RightHandler.RemoveRights(userEntity.Player.Id);
                    await _roomController.TakeRoomRights(room.RoomData.Id, userEntity.Player.Id);
                    await room.RightHandler.ReloadRights(userEntity.Session);
                }
            }
        }
    }
}

