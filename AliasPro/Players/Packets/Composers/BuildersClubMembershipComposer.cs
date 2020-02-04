using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class BuildersClubMembershipComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.BuildersClubMembershipMessageComposer);
            message.WriteInt(int.MaxValue);
            message.WriteInt(100);
            message.WriteInt(0);
            message.WriteInt(int.MaxValue);
            return message;
        }
    }
}
