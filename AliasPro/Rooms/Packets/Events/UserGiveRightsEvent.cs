using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

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

            uint targetId = (uint)clientPacket.ReadInt();
            if (room.Rights.HasRights(targetId)) return;

            if (!_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer))
                return;

            room.Rights.GiveRights(targetPlayer.Id, targetPlayer.Username);
            await _roomController.GiveRoomRights(room.Id, targetPlayer.Id);

            if (targetPlayer.Session != null)
                await room.Rights.ReloadRights(targetPlayer.Session);
        }
    }
}

