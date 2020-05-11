using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Chat.Models;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationRoomChatlogComposer : IMessageComposer
    {
        private readonly IRoom _room;
        private readonly ICollection<ChatLog> _chatLogs;
        
        public ModerationRoomChatlogComposer(
			IRoom room, 
			ICollection<ChatLog> chatLogs)
        {
			_room = room;
            _chatLogs = chatLogs;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationRoomChatlogMessageComposer);
            message.WriteByte(1);
            message.WriteShort(2);
            message.WriteString("roomName");
            message.WriteByte(2);
            message.WriteString(_room.Name);
            message.WriteString("roomId");
            message.WriteByte(1);
            message.WriteInt((int)_room.Id);

            message.WriteShort((short)_chatLogs.Count);
            foreach (ChatLog chatlog in _chatLogs)
                chatlog.Compose(message);

            return message;
        }
    }
}
