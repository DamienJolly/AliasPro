using AliasPro.API.Moderation.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueInfoComposer : IMessageComposer
    {
        private readonly IModerationTicket _ticket;

        public ModerationIssueInfoComposer(IModerationTicket ticket)
        {
            _ticket = ticket;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationIssueInfoMessageComposer);
            _ticket.Compose(message);
            return message;
        }
    }
}
