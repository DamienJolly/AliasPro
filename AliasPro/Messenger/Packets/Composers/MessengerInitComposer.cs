using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Composers
{
    public class MessengerInitComposer : IPacketComposer
    {
        private readonly int _maxFriends;

        public MessengerInitComposer(int maxFriends)
        {
            _maxFriends = maxFriends;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.MessengerInitMessageComposer);
            message.WriteInt(_maxFriends);
            message.WriteInt(300);
            message.WriteInt(800);
            message.WriteInt(0);
            message.WriteBoolean(true);
            return message;
        }
    }
}
