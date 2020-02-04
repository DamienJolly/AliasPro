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
    public class UpdateRoomPromotionEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.UpdateRoomPromotionMessageEvent;

        private readonly IRoomController _roomController;

        public UpdateRoomPromotionEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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
                await room.SendPacketAsync(new RoomPromotionComposer(room));
            }
        }
    }
}
