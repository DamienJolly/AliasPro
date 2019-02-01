using System;
using System.Data.Common;

namespace AliasPro.Navigator.Models
{
    using Database;

    internal class NavigatorCategory : INavigatorCategory
    {
        internal NavigatorCategory(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            MinRank = reader.ReadData<int>("min_rank");
            PublicName = reader.ReadData<string>("public_name");
            Identifier = reader.ReadData<string>("identifier");
            Category = reader.ReadData<string>("category");
            CategoryType = Enum.Parse<CategoryType>(reader.ReadData<string>("category_type"));
        }

        public uint Id { get; set; }
        public int MinRank { get; set; }
        public string PublicName { get; set; }
        public string Identifier { get; set; }
        public string Category { get; set; }
        public CategoryType CategoryType { get; set; }
    }
}
