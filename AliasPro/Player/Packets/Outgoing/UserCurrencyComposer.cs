using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using AliasPro.API.Player.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserCurrencyComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerCurrency> _currencies;

        public UserCurrencyComposer(ICollection<IPlayerCurrency> currencies)
        {
            _currencies = currencies;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserCurrencyMessageComposer);
            message.WriteInt(_currencies.Count);
            foreach (IPlayerCurrency currency in _currencies)
            {
                message.WriteInt(currency.Type);
                message.WriteInt(currency.Amount);
            }
            return message;
        }
    }
}
