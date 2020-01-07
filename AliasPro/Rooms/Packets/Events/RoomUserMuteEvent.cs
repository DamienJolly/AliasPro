using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserMuteEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomUserMuteMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserMuteEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();
            int roomId = clientPacket.ReadInt();
            int minutes = clientPacket.ReadInt();

            if (!_roomController.TryGetRoom((uint)roomId, out IRoom room)) 
                return;

            if (!room.Rights.HasRights(session.Player.Id)) return;

            if (room.Rights.HasRights((uint)playerId)) return;

            if (!room.Entities.TryGetPlayerEntityById(playerId, out PlayerEntity entity))
                return;

            room.Mute.MutePlayer(playerId, (int)UnixTimestamp.Now + (minutes * 60));
            await entity.Session.SendPacketAsync(new MutedWhisperComposer(minutes * 60));
        }
    }
}
