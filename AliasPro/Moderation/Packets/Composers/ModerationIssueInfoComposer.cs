using AliasPro.API.Moderation.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueInfoComposer : IPacketComposer
    {
        private readonly IModerationTicket _ticket;

        public ModerationIssueInfoComposer(IModerationTicket ticket)
        {
            _ticket = ticket;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationIssueInfoMessageComposer);
            _ticket.Compose(message);
            return message;
        }
    }
}
