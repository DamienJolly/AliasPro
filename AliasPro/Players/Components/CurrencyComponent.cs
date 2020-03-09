using AliasPro.API.Players.Models;
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

        public bool TryAddCurrency(IPlayerCurrency currency) => 
            _currencies.TryAdd(currency.Type, currency);

        public bool TryGetCurrency(int type, out IPlayerCurrency currency) =>
            _currencies.TryGetValue(type, out currency);

        public int GetCurrenyAmount(int type) =>
            _currencies.ContainsKey(type) ? _currencies[type].Amount : 0;
    }
}
