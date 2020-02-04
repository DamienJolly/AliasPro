using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationRoomInfoComposer : IMessageComposer
    {
        private readonly IRoom _room;
        
        public ModerationRoomInfoComposer(
			IRoom room)
        {
            _room = room;
        }

        public ServerMessage Compose()
        {
            bool publicRoom = true;
            ServerMessage message = new ServerMessage(Outgoing.ModerationRoomInfoMessageComposer);
            message.WriteInt((int)_room.Id);
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
