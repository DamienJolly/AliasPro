namespace AliasPro.Navigator.Models
{
    public interface INavigatorCategory
    {
        uint Id { get; set; }
        int MinRank { get; set; }
        string PublicName { get; set; }
        string Identifier { get; set; }
        string Category { get; set; }
        CategoryType CategoryType { get; set; }
    }
}
