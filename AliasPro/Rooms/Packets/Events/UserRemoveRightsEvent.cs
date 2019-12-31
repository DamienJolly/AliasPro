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
    public class UserRemoveRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UserRemoveRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public UserRemoveRightsEvent(
			IPlayerController playerController, 
			IRoomController roomController)
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

            int amount = clientPacket.ReadInt();

            for (int i = 0; i < amount; i++)
            {
                uint targetId = (uint)clientPacket.ReadInt();
                if (!room.Rights.HasRights(targetId)) continue;

                room.Rights.RemoveRights(targetId);
                await _roomController.TakeRoomRights(room.Id, targetId);

                if (_playerController.TryGetPlayer(targetId, out IPlayer targetPlayer) && targetPlayer.Session != null)
                    await room.Rights.ReloadRights(targetPlayer.Session);
            }
        }
    }
}

