using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class BuildersClubMembershipComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.BuildersClubMembershipMessageComposer);
            message.WriteInt(int.MaxValue);
            message.WriteInt(100);
            message.WriteInt(0);
            message.WriteInt(int.MaxValue);
            return message;
        }
    }
}
