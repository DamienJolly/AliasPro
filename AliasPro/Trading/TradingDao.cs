using AliasPro.API.Configuration;
using AliasPro.API.Database;
using System.Threading.Tasks;

namespace AliasPro.Trading
{
    internal class TradingDao : BaseDao
    {
        public TradingDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }
	}
}
