namespace AliasPro.Catalog
{
    internal class CatalogRepostiory
    {
        private readonly CatalogDao _catalogDao;

        public CatalogRepostiory(CatalogDao catalogDao)
        {
            _catalogDao = catalogDao;
        }
    }
}
