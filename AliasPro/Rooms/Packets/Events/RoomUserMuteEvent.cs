using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserMuteEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserMuteMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserMuteEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();
            int roomId = message.ReadInt();
            int minutes = message.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                return Task.CompletedTask;

            switch (room.Settings.WhoMutes)
            {
                case 0: default: if (!room.Rights.IsOwner(session.Player.Id)) return Task.CompletedTask; break;
                case 1: if (!room.Rights.HasRights(session.Player.Id)) return Task.CompletedTask; break;
            }

            if (room.Rights.HasRights((uint)playerId)) 
                return Task.CompletedTask;

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
                return Task.CompletedTask;

            room.Mute.MutePlayer(playerId, (int)UnixTimestamp.Now + (minutes * 60));

            return Task.CompletedTask;
        }
    }
}
