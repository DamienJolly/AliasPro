using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Layouts
{
	public abstract class CatalogLayout
	{
		public CatalogPage Page;

		public CatalogLayout(CatalogPage page)
		{
			Page = page;
		}

		public abstract void ComposePageData(ServerMessage message);
	}
}
