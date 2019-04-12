using AliasPro.API.Network.Events;
using AliasPro.API.Player.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
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
