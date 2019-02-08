using System.Collections.Generic;

namespace AliasPro.Player.Models.Currency
{
    public class PlayerCurrency
    {
        private readonly IDictionary<int, ICurrencyType> _currencies;

        internal PlayerCurrency(IDictionary<int, ICurrencyType> currencies)
        {
            _currencies = currencies;
        }

        public ICollection<ICurrencyType> Currencies =>
            _currencies.Values;
    }
}
