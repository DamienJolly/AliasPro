namespace AliasPro.Figure
{
    using Configuration;
    using Database;

    internal class FigureDao : BaseDao
    {
        public FigureDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }
    }
}