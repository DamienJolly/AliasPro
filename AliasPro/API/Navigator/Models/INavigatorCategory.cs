using AliasPro.API.Navigator.Views;

namespace AliasPro.API.Navigator.Models
{
    public interface INavigatorCategory
    {
        int SortId { get; set; }
        int MinRank { get; set; }
        string PublicName { get; set; }
        string Identifier { get; set; }
        string View { get; set; }
        ICategoryType CategoryType { get; set; }
        bool Enabled { get; set; }
    }
}
