using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationPickTicketEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationPickTicketMessageEvent;

		private readonly IModerationController _moderationController;
		private readonly IPermissionsController _permissionsController;

		public ModerationPickTicketEvent(
			IModerationController moderationController,
			IPermissionsController permissionsController)
		{
			_moderationController = moderationController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_ticket_queue"))
				return;

			int count = message.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int ticketId = message.ReadInt();
                if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                    continue;

                if (ticket.State != ModerationTicketState.OPEN)
                    continue;

                ticket.ModId = (int)session.Player.Id;
                ticket.ModUsername = session.Player.Username;
                ticket.State = ModerationTicketState.PICKED;

                await _moderationController.UpdateTicket(ticket);
                await session.SendPacketAsync(new ModerationIssueInfoComposer(ticket));
            }
        }
    }
}