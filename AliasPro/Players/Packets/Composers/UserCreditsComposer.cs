using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserCreditsComposer : IPacketComposer
    {
        private readonly int _credits;

        public UserCreditsComposer(int credits)
        {
            _credits = credits;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserCreditsMessageComposer);
            message.WriteString(_credits + ".0");
            return message;
        }
    }
}
