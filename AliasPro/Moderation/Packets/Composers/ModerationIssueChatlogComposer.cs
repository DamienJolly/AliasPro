using AliasPro.API.Chat.Models;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueChatlogComposer : IPacketComposer
    {
        private readonly IModerationTicket _ticket;
        private readonly ICollection<IChatLog> _chatLogs;
        
        public ModerationIssueChatlogComposer(IModerationTicket ticket, ICollection<IChatLog> chatLogs)
        {
            _ticket = ticket;
            _chatLogs = chatLogs;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationIssueChatlogMessageComposer);
            message.WriteInt(_ticket.Id);
            message.WriteInt(_ticket.SenderId);
            message.WriteInt(_ticket.ReportedId);
            message.WriteInt(_ticket.RoomId);

            message.WriteByte(1); //1 = room report, 2 = im session, 3 = forum thread, 4 = forum message, 5 = selfie report, 6 = photo report
            message.WriteShort(2);
            message.WriteString("roomName");
            message.WriteByte(2);
            message.WriteString("Unknown");
            message.WriteString("roomId");
            message.WriteByte(1);
            message.WriteInt(_ticket.RoomId);

            message.WriteShort(_chatLogs.Count);
            foreach (IChatLog chatlog in _chatLogs)
                chatlog.Compose(message);

            return message;
        }
    }
}
