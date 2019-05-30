using AliasPro.API.Chat.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationRoomChatlogComposer : IPacketComposer
    {
        private readonly IRoom _room;
        private readonly ICollection<IChatLog> _chatLogs;
        
        public ModerationRoomChatlogComposer(
			IRoom room, 
			ICollection<IChatLog> chatLogs)
        {
			_room = room;
            _chatLogs = chatLogs;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationRoomChatlogMessageComposer);
            message.WriteByte(1);
            message.WriteShort(2);
            message.WriteString("roomName");
            message.WriteByte(2);
            message.WriteString(_room.Name);
            message.WriteString("roomId");
            message.WriteByte(1);
            message.WriteInt(_room.Id);

            message.WriteShort(_chatLogs.Count);
            foreach (IChatLog chatlog in _chatLogs)
                chatlog.Compose(message);

            return message;
        }
    }
}
