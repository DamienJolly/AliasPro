using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class UserCurrencyComposer : IMessageComposer
    {
        private readonly ICollection<IPlayerCurrency> _currencies;

        public UserCurrencyComposer(ICollection<IPlayerCurrency> currencies)
        {
            _currencies = currencies;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserCurrencyMessageComposer);
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
