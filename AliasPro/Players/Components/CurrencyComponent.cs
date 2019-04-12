using AliasPro.API.Player.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class CurrencyComponent
    {
        private readonly IDictionary<int, IPlayerCurrency> _currencies;

        internal CurrencyComponent(
            IDictionary<int, IPlayerCurrency> currencies)
        {
            _currencies = currencies;
        }

        public ICollection<IPlayerCurrency> Currencies =>
            _currencies.Values;
    }
}
