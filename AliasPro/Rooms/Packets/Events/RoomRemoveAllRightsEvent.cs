using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;
using System.Linq;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomRemoveAllRightsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomRemoveAllRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public RoomRemoveAllRightsEvent(
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

            int roomId = clientPacket.ReadInt(); //unused?
            if (room.Id != (uint)roomId) return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            foreach (var right in room.Rights.Rights.ToList())
            {
                room.Rights.RemoveRights(right.Key);
                await _roomController.TakeRoomRights(room.Id, right.Key);

                if (_playerController.TryGetPlayer(right.Key, out IPlayer targetPlayer) && targetPlayer.Session != null)
                    await room.Rights.ReloadRights(targetPlayer.Session);

                await room.SendAsync(new RoomRemoveRightsListComposer((int)room.Id, (int)right.Key));
            }
        }
    }
}

