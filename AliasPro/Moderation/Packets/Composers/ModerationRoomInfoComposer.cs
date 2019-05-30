using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationRoomInfoComposer : IPacketComposer
    {
        private readonly IRoom _room;
        
        public ModerationRoomInfoComposer(
			IRoom room)
        {
            _room = room;
        }

        public ServerPacket Compose()
        {
            bool publicRoom = true;
            ServerPacket message = new ServerPacket(Outgoing.ModerationRoomInfoMessageComposer);
            message.WriteInt(_room.Id);
            message.WriteInt(_room.UsersNow);
            message.WriteBoolean(true); //todo: owner online
            message.WriteInt(_room.OwnerId);
            message.WriteString(_room.OwnerName);
            message.WriteBoolean(publicRoom); //todo: not public room
            if (publicRoom)
            {
                message.WriteString(_room.Name);
                message.WriteString(_room.Description);
                message.WriteInt(_room.Tags.Count);
                foreach (string tag in _room.Tags)
                    message.WriteString(tag);
            }
            return message;
        }
    }
}
