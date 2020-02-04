using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomPromotionComposer : IMessageComposer
    {
        private readonly IRoomData _roomData;

        public RoomPromotionComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomPromotionMessageComposer);
            message.WriteInt(!_roomData.IsPromoted ? -1 : (int)_roomData.Id);
            message.WriteInt(!_roomData.IsPromoted ? -1 : _roomData.OwnerId);
            message.WriteString(!_roomData.IsPromoted ? "" : _roomData.OwnerName);
            message.WriteInt(!_roomData.IsPromoted ? 0 : 1);
            message.WriteInt(!_roomData.IsPromoted ? 0 : 1);
            message.WriteString(!_roomData.IsPromoted ? "" : _roomData.Promotion.Title);
            message.WriteString(!_roomData.IsPromoted ? "" : _roomData.Promotion.Description);
            message.WriteInt(0); // dunno?
            message.WriteInt(0); // dunno?
            message.WriteInt(0); // dunno?
            return message;
        }
    }
}
