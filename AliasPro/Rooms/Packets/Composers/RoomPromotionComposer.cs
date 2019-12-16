using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomPromotionComposer : IPacketComposer
    {
        private readonly IRoomData _roomData;

        public RoomPromotionComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomPromotionMessageComposer);
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
