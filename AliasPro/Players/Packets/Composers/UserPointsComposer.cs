using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserPointsComposer : IPacketComposer
    {
        private readonly int _amount;
        private readonly int _amountAdded;
        private readonly int _type;

        public UserPointsComposer(int amount, int amountAdded, int type)
        {
            _amount = amount;
            _amountAdded = amountAdded;
            _type = type;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserPointsMessageComposer);
            message.WriteInt(_amount);
            message.WriteInt(_amountAdded);
            message.WriteInt(_type);
            return message;
        }
    }
}
