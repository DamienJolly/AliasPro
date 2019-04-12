namespace AliasPro.API.Landing.Models
{
    public interface IArticle
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
