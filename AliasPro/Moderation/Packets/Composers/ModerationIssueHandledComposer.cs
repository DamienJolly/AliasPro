using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueHandledComposer : IPacketComposer
    {
        private readonly int _code;
        
        public ModerationIssueHandledComposer(int code)
        {
            _code = code;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationIssueHandledMessageComposer);
            message.WriteInt(_code);
            message.WriteString("");
            return message;
        }
    }
}
