using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Packets.Composers;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomRemoveAllRightsEvent : IMessageEvent
    {
        public short Header => Incoming.RoomRemoveAllRightsMessageEvent;

        private readonly IPlayerController _playerController;
        private readonly IRoomController _roomController;

        public RoomRemoveAllRightsEvent(
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
            if (room == null || session.Entity == null) return;

            int roomId = message.ReadInt(); //unused?
            if (room.Id != (uint)roomId) return;

            if (!room.Rights.IsOwner(session.Player.Id)) return;

            foreach (var right in room.Rights.Rights.ToList())
            {
                room.Rights.RemoveRights(right.Key);
                await _roomController.TakeRoomRights(room.Id, right.Key);

                if (_playerController.TryGetPlayer(right.Key, out IPlayer targetPlayer) && targetPlayer.Session != null)
                    await room.Rights.ReloadRights(targetPlayer.Session);

                await room.SendPacketAsync(new RoomRemoveRightsListComposer((int)room.Id, (int)right.Key));
            }
        }
    }
}

