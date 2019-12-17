using AliasPro.API.Configuration;
using AliasPro.API.Database;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AliasPro.Trading
{
    internal class TradingDao : BaseDao
    {
        public TradingDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
			: base(logger, configurationController)
		{

		}
	}
}
