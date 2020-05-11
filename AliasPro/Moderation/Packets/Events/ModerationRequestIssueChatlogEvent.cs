using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Chat;
using AliasPro.Game.Chat.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestIssueChatlogEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationRequestIssueChatlogMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly ChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestIssueChatlogEvent(
			IModerationController moderationController,
			ChatController chatController,
			IPermissionsController permissionsController)
		{
			_moderationController = moderationController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_ticket_queue"))
				return;

			int ticketId = message.ReadInt();
            if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                return;

			ModerationChatlogType chatlogType;
			ICollection<ChatLog> chatlogs;

			switch (ticket.Type)
			{
				case ModerationTicketType.IM:
					{
						chatlogType = ModerationChatlogType.IM;
						chatlogs = await _chatController.ReadMessengerChatlogs((uint)ticket.ReportedId, (uint)ticket.SenderId);
					}
					break;
				default:
					{
						chatlogType = ModerationChatlogType.CHAT;
						if (ticket.RoomId != 0)
							chatlogs = await _chatController.ReadRoomChatlogs((uint)ticket.RoomId);
						else
							chatlogs = await _chatController.ReadUserChatlogs((uint)ticket.ReportedId);
					}
					break;
			}

            await session.SendPacketAsync(new ModerationIssueChatlogComposer(ticket, chatlogType, chatlogs));
        }
    }
}