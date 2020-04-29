using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
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

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();
            int roomId = message.ReadInt();
            int minutes = message.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room))
                return;

            switch (room.Settings.WhoMutes)
            {
                case 0: default: if (!room.Rights.IsOwner(session.Player.Id)) return; break;
                case 1: if (!room.Rights.HasRights(session.Player.Id)) return; break;
            }

            if (room.Rights.HasRights((uint)playerId)) 
                return;

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
                return;

            room.Mute.MutePlayer((int)entity.Player.Id, (int)UnixTimestamp.Now + (minutes * 60));
            await entity.Player.Session.SendPacketAsync(new MutedWhisperComposer(minutes * 60));
        }
    }
}
