using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class UserPointsComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserPointsMessageComposer);
            message.WriteInt(_amount);
            message.WriteInt(_amountAdded);
            message.WriteInt(_type);
            return message;
        }
    }
}
