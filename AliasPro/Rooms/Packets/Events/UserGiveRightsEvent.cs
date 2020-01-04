using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

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

            int targetId = clientPacket.ReadInt();
            if (room.Rights.HasRights((uint)targetId)) return;

            string username;
            bool reloadRights = false;
            if (_playerController.TryGetPlayer((uint)targetId, out IPlayer targetPlayer))
            {
                username = targetPlayer.Username;
                reloadRights = true;
            }
            else
            {
                if (session.Player.Messenger == null) 
                    return;

                if (!session.Player.Messenger.TryGetFriend((uint)targetId, out IMessengerFriend friend))
                    return;

                username = friend.Username;
            }

            room.Rights.GiveRights((uint)targetId, username);
            await _roomController.GiveRoomRights(room.Id, (uint)targetId);

            if (reloadRights)
                await room.Rights.ReloadRights(targetPlayer.Session);

            await room.SendAsync(new RoomAddRightsListComposer((int)room.Id, targetId, username));
        }
    }
}

