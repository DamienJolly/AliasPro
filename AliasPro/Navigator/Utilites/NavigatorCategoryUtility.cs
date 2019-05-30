using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.Navigator.Views;

namespace AliasPro.Navigator.Utilites
{
    public class NavigatorCategoryUtility
    {
        public static ICategoryType GetCategoryType(string categoryType, INavigatorCategory category)
        {
            switch (categoryType)
            {
                case "category": default: return new DefaultCategory(category);
                case "query": return new QueryCategory(category);
                case "popular": return new PopularCategory(category);
                case "top_promotions": return new TopPromotionsCategory(category);
                case "promotion_category": return new PromotionsCategory(category);
                case "my_rooms": return new MyRoomsCategory(category);
            }
        }
    }
}
