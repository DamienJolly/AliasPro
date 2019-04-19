using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationRoomInfoComposer : IPacketComposer
    {
        private readonly IRoomData _roomData;
        
        public ModerationRoomInfoComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerPacket Compose()
        {
            bool publicRoom = true;
            ServerPacket message = new ServerPacket(Outgoing.ModerationRoomInfoMessageComposer);
            message.WriteInt(_roomData.Id);
            message.WriteInt(_roomData.UsersNow);
            message.WriteBoolean(true); //todo: owner online
            message.WriteInt(_roomData.OwnerId);
            message.WriteString(_roomData.OwnerName);
            message.WriteBoolean(publicRoom); //todo: not public room
            if (publicRoom)
            {
                message.WriteString(_roomData.Name);
                message.WriteString(_roomData.Description);
                message.WriteInt(_roomData.Tags.Count);
                foreach (string tag in _roomData.Tags)
                    message.WriteString(tag);
            }
            return message;
        }
    }
}
