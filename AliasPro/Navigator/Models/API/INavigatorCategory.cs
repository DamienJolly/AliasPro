namespace AliasPro.Navigator.Models
{
    using Views;

    public interface INavigatorCategory
    {
        uint Id { get; set; }
        int MinRank { get; set; }
        string PublicName { get; set; }
        string Identifier { get; set; }
        string Category { get; set; }
        ICategoryType CategoryType { get; set; }
    }
}
