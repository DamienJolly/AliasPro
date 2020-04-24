using AliasPro.API.Currency.Models;
using System.Collections.Generic;

namespace AliasPro.API.Currency
{
	public interface ICurrencyController
	{
		ICollection<ICurrencySetting> CurrencySettings { get; }

		void InitializeCurrency();
		bool TryGetCurrency(int currencyId, out ICurrencySetting currency);
	}
}
