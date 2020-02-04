using AliasPro.API.Chat;
using AliasPro.API.Chat.Models;
using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationRequestIssueChatlogEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.ModerationRequestIssueChatlogMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly IChatController _chatController;
		private readonly IPermissionsController _permissionsController;

		public ModerationRequestIssueChatlogEvent(
			IModerationController moderationController,
			IChatController chatController,
			IPermissionsController permissionsController)
		{
			_moderationController = moderationController;
			_chatController = chatController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_ticket_queue"))
				return;

			int ticketId = clientPacket.ReadInt();
            if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                return;

			ICollection<IChatLog> chatlogs;
            if (ticket.RoomId != 0)
                chatlogs = await _chatController.ReadRoomChatlogs((uint)ticket.RoomId);
            else
                chatlogs = await _chatController.ReadUserChatlogs((uint)ticket.ReportedId);

            await session.SendPacketAsync(new ModerationIssueChatlogComposer(ticket, chatlogs));
        }
    }
}