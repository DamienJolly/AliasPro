﻿using AliasPro.API.Moderation;
using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Moderation.Packets.Composers;
using AliasPro.Moderation.Types;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Moderation.Packets.Events
{
    public class ModerationReleaseTicketEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.ModerationReleaseTicketMessageEvent;

        private readonly IModerationController _moderationController;

        public ModerationReleaseTicketEvent(IModerationController moderationController)
        {
            _moderationController = moderationController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: permissions
            if (session.Player.Rank <= 2)
                return;

            int count = clientPacket.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int ticketId = clientPacket.ReadInt();
                if (!_moderationController.TryGetTicket(ticketId, out IModerationTicket ticket))
                    continue;
                
                ticket.ModId = 0;
                ticket.ModUsername = "";
                ticket.State = ModerationTicketState.OPEN;

                await _moderationController.UpdateTicket(ticket);
                await session.SendPacketAsync(new ModerationIssueInfoComposer(ticket));
            }
        }
    }
}