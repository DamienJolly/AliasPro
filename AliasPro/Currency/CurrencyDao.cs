using AliasPro.API.Currency.Models;
using AliasPro.API.Database;
using AliasPro.Currency.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Currency
{
	internal class CurrencyDao : BaseDao
	{
		public CurrencyDao(ILogger<BaseDao> logger)
			: base(logger)
		{

		}

        internal async Task<IDictionary<int, ICurrencySetting>> GetCurrencySettings()
        {
            IDictionary<int, ICurrencySetting> settings = new Dictionary<int, ICurrencySetting>();
            await CreateTransaction(async transaction =>
            {
                await Select(transaction, async reader =>
                {
                    while (await reader.ReadAsync())
                    {
                        ICurrencySetting setting = new CurrencySetting(reader);
                        settings.TryAdd(setting.Id, setting);
                    }
                }, "SELECT * FROM `currency_settings`;");
            });
            return settings;
        }
    }
}
