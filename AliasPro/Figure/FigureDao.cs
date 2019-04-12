using AliasPro.API.Configuration;
using AliasPro.Database;

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