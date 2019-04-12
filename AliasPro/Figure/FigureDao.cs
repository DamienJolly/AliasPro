using AliasPro.API.Configuration;
using AliasPro.API.Database;

namespace AliasPro.Figure
{
    internal class FigureDao : BaseDao
    {
        public FigureDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }
    }
}