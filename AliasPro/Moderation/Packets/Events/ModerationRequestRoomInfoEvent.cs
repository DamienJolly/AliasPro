using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestRoomInfoEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestRoomInfoMessageEvent;
        
        private readonly IRoomController _roomController;

        public ModerationRequestRoomInfoEvent(
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

            await session.SendPacketAsync(new ModerationRoomInfoComposer(roomData));
        }
    }
}