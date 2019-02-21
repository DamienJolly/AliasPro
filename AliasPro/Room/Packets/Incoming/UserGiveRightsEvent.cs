using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Room.Models;
    using Room.Models.Entities;
    using Player;

    public class UserGiveRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserGiveRightsMessageEvent;

        private readonly IPlayerController _playerController;

        public UserGiveRightsEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) return;

            if (!room.RightHandler.IsOwner(session.Player.Id)) return;

            int entityId = clientPacket.ReadInt();

            if (!room.EntityHandler.TryGetEntityById(entityId, out BaseEntity entity)) return;

            if (entity is UserEntity userEntity)
            {
                //if (room.RightHandler.HasRights(userEntity.Player.Id)) return;
                
                room.RightHandler.GiveRights(userEntity.Player);
                await room.RightHandler.ReloadRights(userEntity.Session);
            }
        }
    }
}

