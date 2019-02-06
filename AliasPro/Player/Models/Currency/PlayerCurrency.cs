using System;
using System.Collections.Generic;

namespace AliasPro.Player.Models.Currency
{
    public class PlayerCurrency : IDisposable
    {
        private readonly IDictionary<int, ICurrencyType> _currencies;

        internal PlayerCurrency(IDictionary<int, ICurrencyType> currencies)
        {
            _currencies = currencies;
        }

        public ICollection<ICurrencyType> Currencies =>
            _currencies.Values;

        public void Dispose()
        {
            //todo: save currencies
        }
    }
}
