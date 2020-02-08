using AliasPro.API.Messenger.Models;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class UserGiveRightsEvent : IMessageEvent
    {
        public short Header => Incoming.UserGiveRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public UserGiveRightsEvent(IPlayerController playerController, IRoomController roomController)
        {
            _playerController = playerController;
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) 
                return;

            int targetId = clientPacket.ReadInt();
            if (room.Rights.HasRights((uint)targetId)) 
                return;

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

            await room.SendPacketAsync(new RoomAddRightsListComposer((int)room.Id, targetId, username));
        }
    }
}

