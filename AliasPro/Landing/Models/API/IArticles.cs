namespace AliasPro.Landing.Models
{
    public interface IArticles
    {
        uint Id { get; set; }
        string Title { get; set; }
        string Message { get; set; }
        string Caption { get; set; }
        int Type { get; set; }
        string Link { get; set; }
        string Image { get; set; }
    }
}
