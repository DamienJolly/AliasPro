using AliasPro.API.Chat;
using AliasPro.API.Chat.Models;
using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Permissions;
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

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
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