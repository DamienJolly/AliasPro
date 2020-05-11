using AliasPro.Game.Catalog.Layouts;
using AliasPro.Game.Catalog.Models;

namespace AliasPro.Game.Catalog.Utilities
{
	public class CatalogLayoutUtility
	{
        public static CatalogLayout GetCatalogLayout(string layout, CatalogPage catalogPage)
        {
            switch (layout.ToLower())
            {
                default: 
                    return new LayoutDefault(catalogPage);

                case "frontpage": 
                    return new LayoutFrontpage(catalogPage);

                case "guilds": 
                    return new LayoutGroup(catalogPage);

                case "badge_display": 
                    return new LayoutBadgeDisplay(catalogPage);

                case "trophies": 
                    return new LayoutTrophies(catalogPage);

                case "bot": 
                    return new LayoutBot(catalogPage);

                case "pets": 
                    return new LayoutPets(catalogPage);

                case "pets3": 
                    return new LayoutPets3(catalogPage);

                case "roomads":
                    return new LayoutRoomAds(catalogPage);

                case "spaces_new": 
                    return new LayoutSpacesNew(catalogPage);

                case "recycler":
                    return new LayoutRecycler(catalogPage);

                case "recycler_prizes": 
                    return new LayoutRecyclerPrizes(catalogPage);

                case "recycler_info":
                    return new LayoutRecyclerInfo(catalogPage);

                case "marketplace": 
                    return new LayoutMarketplace(catalogPage);

                case "marketplace_own_items": 
                    return new LayoutMarketplaceOwnItems(catalogPage);

                case "default_3x3_color_grouping":
                    return new LayoutColorGrouping(catalogPage);

                case "soundmachine": 
                    return new LayoutTrax(catalogPage);
            }
        }
    }
}
