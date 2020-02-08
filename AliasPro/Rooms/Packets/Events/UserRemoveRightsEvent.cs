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
    public class UserRemoveRightsEvent : IMessageEvent
    {
        public short Header => Incoming.UserRemoveRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public UserRemoveRightsEvent(
			IPlayerController playerController, 
			IRoomController roomController)
        {
            _playerController = playerController;
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null || session.Entity == null) 
                return;

            if (!room.Rights.IsOwner(session.Player.Id)) 
                return;

            int amount = message.ReadInt();

            for (int i = 0; i < amount; i++)
            {
                int targetId = message.ReadInt();
                if (!room.Rights.HasRights((uint)targetId)) 
                    continue;

                room.Rights.RemoveRights((uint)targetId);
                await _roomController.TakeRoomRights(room.Id, (uint)targetId);

                if (_playerController.TryGetPlayer((uint)targetId, out IPlayer targetPlayer) && targetPlayer.Session != null)
                    await room.Rights.ReloadRights(targetPlayer.Session);

                await room.SendPacketAsync(new RoomRemoveRightsListComposer((int)room.Id, targetId));
            }
        }
    }
}

