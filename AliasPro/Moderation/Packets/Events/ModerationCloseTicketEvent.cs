using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using System.Threading.Tasks;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationCloseTicketEvent : IMessageEvent
    {
        public short Header => Incoming.ModerationCloseTicketMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly IPlayerController _playerController;
		private readonly IPermissionsController _permissionsController;

		public ModerationCloseTicketEvent(
			IModerationController moderationController,
			IPlayerController playerController,
			IPermissionsController permissionsController)
		{
			_moderationController = moderationController;
			_playerController = playerController;
			_permissionsController = permissionsController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			if (!_permissionsController.HasPermission(session.Player, "acc_modtool_ticket_queue"))
				return;

			int state = clientPacket.ReadInt();
            int count = clientPacket.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int ticketId = clientPacket.ReadInt();
                if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                    continue;

                if (_playerController.TryGetPlayer((uint)ticket.SenderId, out IPlayer player))
                {
                    if (player.Session != null)
                        await player.Session.SendPacketAsync(new ModerationIssueHandledComposer(state));
                }
                
                ticket.State = ModerationTicketState.CLOSED;

                await _moderationController.UpdateTicket(ticket);
                _moderationController.RemoveTicket(ticket.Id);
                await session.SendPacketAsync(new ModerationIssueInfoComposer(ticket));
            }
        }
    }
}