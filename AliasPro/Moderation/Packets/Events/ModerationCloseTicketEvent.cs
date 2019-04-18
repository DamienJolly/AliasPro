using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationCloseTicketEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationCloseTicketMessageEvent;

        private readonly IModerationController _moderationController;
        private readonly IPlayerController _playerController;

        public ModerationCloseTicketEvent(IModerationController moderationController, IPlayerController playerController)
        {
            _moderationController = moderationController;
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
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