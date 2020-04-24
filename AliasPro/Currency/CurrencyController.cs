using AliasPro.API.Currency;
using AliasPro.API.Currency.Models;
using System.Collections.Generic;

namespace AliasPro.Currency
{
	internal class CurrencyController : ICurrencyController
	{
		private readonly CurrencyDao _currencyDao;

		private IDictionary<int, ICurrencySetting> _currencySettings;

		public CurrencyController(CurrencyDao currencyDao)
		{
			_currencyDao = currencyDao;
			_currencySettings = new Dictionary<int, ICurrencySetting>();

			InitializeCurrency();
		}

		public async void InitializeCurrency()
		{
			_currencySettings = await _currencyDao.GetCurrencySettings();
		}

		public ICollection<ICurrencySetting> CurrencySettings =>
			_currencySettings.Values;

		public bool TryGetCurrency(int currencyId, out ICurrencySetting currency) =>
			_currencySettings.TryGetValue(currencyId, out currency);
	}
}
