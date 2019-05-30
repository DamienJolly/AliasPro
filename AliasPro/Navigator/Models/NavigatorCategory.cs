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
            Id = reader.ReadData<uint>("id");
            MinRank = reader.ReadData<int>("min_rank");
            PublicName = reader.ReadData<string>("public_name");
            Identifier = reader.ReadData<string>("identifier");
            Category = reader.ReadData<string>("category");
            CategoryType = NavigatorCategoryUtility.GetCategoryType(
                reader.ReadData<string>("category_type"), this);
        }

        public uint Id { get; set; }
        public int MinRank { get; set; }
        public string PublicName { get; set; }
        public string Identifier { get; set; }
        public string Category { get; set; }
        public ICategoryType CategoryType { get; set; }
    }
}
