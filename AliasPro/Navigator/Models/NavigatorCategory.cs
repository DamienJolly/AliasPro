using AliasPro.API.Database;
using AliasPro.API.Navigator.Models;
using AliasPro.API.Navigator.Views;
using AliasPro.Navigator.Utilites;
using System.Data.Common;

namespace AliasPro.Navigator.Models
{
    internal class NavigatorCategory : INavigatorCategory
    {
        internal NavigatorCategory(DbDataReader reader)
        {
            SortId = reader.ReadData<int>("sort_id");
            MinRank = reader.ReadData<int>("min_rank");
            PublicName = reader.ReadData<string>("public_name");
            Identifier = reader.ReadData<string>("identifier");
            View = reader.ReadData<string>("view");
            CategoryType = NavigatorCategoryUtility.GetCategoryType(
                reader.ReadData<string>("category_type"), this);
            Enabled = reader.ReadData<bool>("enabled");
        }

        public int SortId { get; set; }
        public int MinRank { get; set; }
        public string PublicName { get; set; }
        public string Identifier { get; set; }
        public string View { get; set; }
        public ICategoryType CategoryType { get; set; }
        public bool Enabled { get; set; }
    }
}
