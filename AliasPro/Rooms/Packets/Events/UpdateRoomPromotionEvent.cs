using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class UpdateRoomPromotionEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.UpdateRoomPromotionMessageEvent;

        private readonly IRoomController _roomController;

        public UpdateRoomPromotionEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int roomId = clientPacket.ReadInt();
            string title = clientPacket.ReadString();
            string description = clientPacket.ReadString();

            //todo: load room data
            if (!_roomController.TryGetRoom((uint)roomId, out IRoom roomData))
                return;

            if (!roomData.Rights.HasRights(session.Player.Id))
                return;

            if (!roomData.IsPromoted)
                return;

            roomData.Promotion.Title = title;
            roomData.Promotion.Description = description;
            await _roomController.UpdateRoomPromotion(roomData.Promotion);

            if (_roomController.TryGetRoom(roomData.Id, out IRoom room))
            {
                room.Promotion = roomData.Promotion;
                await room.SendAsync(new RoomPromotionComposer(room));
            }
        }
    }
}
