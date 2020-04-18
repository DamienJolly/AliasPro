namespace AliasPro.API.Items.Models
{
    public interface ISongData
    {
        int Id { get; }
        string Code { get; }
        string Name { get; }
        string Author { get; }
        string Track { get; }
        int Length { get; }
    }
}
