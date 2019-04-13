using AliasPro.API.Navigator.Views;
using AliasPro.Navigator.Views;

namespace AliasPro.Navigator.Utilites
{
    public class NavigatorCategoryUtility
    {
        public static ICategoryType GetCategoryType(string categoryType)
        {
            switch (categoryType)
            {
                case "category": default: return new DefaultCategory();
                case "query": return new QueryCategory();
                case "popular": return new PopularCategory();
                case "top_promotions": return new TopPromotionsCategory();
                case "promotion_category": return new PromotionsCategory();
                case "my_rooms": return new MyRoomsCategory();
            }
        }
    }
}
