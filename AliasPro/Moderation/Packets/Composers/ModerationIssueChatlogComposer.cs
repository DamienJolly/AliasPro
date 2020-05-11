using AliasPro.API.Moderation.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Chat.Models;
using AliasPro.Moderation.Types;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueChatlogComposer : IMessageComposer
    {
        private readonly IModerationTicket _ticket;
        private readonly ModerationChatlogType _chatlogType;
        private readonly ICollection<ChatLog> _chatLogs;
        
        public ModerationIssueChatlogComposer(
            IModerationTicket ticket,
            ModerationChatlogType chatlogType,
            ICollection<ChatLog> chatLogs)
        {
            _ticket = ticket;
            _chatlogType = chatlogType;
            _chatLogs = chatLogs;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationIssueChatlogMessageComposer);
            message.WriteInt(_ticket.Id);
            message.WriteInt(_ticket.SenderId);
            message.WriteInt(_ticket.ReportedId);
            message.WriteInt(_ticket.RoomId);

            message.WriteByte((byte)_chatlogType);

            switch (_chatlogType)
            {
                case ModerationChatlogType.IM:
                    message.WriteShort(1);

                    message.WriteString("messageId");
                    message.WriteByte(1);
                    message.WriteInt(_ticket.SenderId);
                    break;
                default:
                    message.WriteShort(3);

                    message.WriteString("roomName");
                    message.WriteByte(2);
                    message.WriteString("Unknown");

                    message.WriteString("roomId");
                    message.WriteByte(1);
                    message.WriteInt(_ticket.RoomId);

                    message.WriteString("groupId");
                    message.WriteByte(1);
                    message.WriteInt(0);
                    break;
            }

            message.WriteShort((short)_chatLogs.Count);
            foreach (ChatLog chatlog in _chatLogs)
                chatlog.Compose(message);

            return message;
        }
    }
}
