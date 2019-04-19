using AliasPro.API.Chat;
using AliasPro.API.Chat.Models;
using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestIssueChatlogEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationRequestIssueChatlogMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly IRoomController _roomController;
        private readonly IChatController _chatController;

        public ModerationRequestIssueChatlogEvent(
            IModerationController moderationController, 
            IRoomController roomController, 
            IChatController chatController)
        {
            _moderationController = moderationController;
            _roomController = roomController;
            _chatController = chatController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;

            int ticketId = clientPacket.ReadInt();
            if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                return;
            
            ICollection<IChatLog> chatlogs = new List<IChatLog>();
            if (ticket.RoomId != 0)
                chatlogs = await _chatController.ReadRoomChatlogs((uint)ticket.RoomId);
            else
                chatlogs = await _chatController.ReadUserChatlogs((uint)ticket.ReportedId);

            await session.SendPacketAsync(new ModerationIssueChatlogComposer(ticket, chatlogs));
        }
    }
}