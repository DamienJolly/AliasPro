using AliasPro.API.Database;
using Microsoft.Extensions.Logging;

namespace AliasPro.Trading
{
    internal class TradingDao : BaseDao
    {
        public TradingDao(ILogger<BaseDao> logger)
			: base(logger)
		{

		}
	}
}
