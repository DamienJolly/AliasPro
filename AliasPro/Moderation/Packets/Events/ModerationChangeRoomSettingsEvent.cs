using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationChangeRoomSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationChangeRoomSettingsMessageEvent;
        
        private readonly IRoomController _roomController;

        public ModerationChangeRoomSettingsEvent(
            IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;
            
            int roomId = clientPacket.ReadInt();

            IRoomData roomData = await _roomController.ReadRoomDataAsync((uint)roomId);
            if (roomData == null)
                return;

            bool lockDoor = clientPacket.ReadInt() == 1;
            bool changeTitle = clientPacket.ReadInt() == 1;
            bool kickUsers = clientPacket.ReadInt() == 1;

            //todo:
        }
    }
}