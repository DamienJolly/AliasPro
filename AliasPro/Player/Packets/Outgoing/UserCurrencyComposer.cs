using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Models.Currency;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserCurrencyComposer : IPacketComposer
    {
        private readonly ICollection<ICurrencyType> _currencies;

        public UserCurrencyComposer(ICollection<ICurrencyType> currencies)
        {
            _currencies = currencies;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserCurrencyMessageComposer);
            message.WriteInt(_currencies.Count);
            foreach (ICurrencyType currency in _currencies)
            {
                message.WriteInt(currency.Type);
                message.WriteInt(currency.Amount);
            }
            return message;
        }
    }
}
