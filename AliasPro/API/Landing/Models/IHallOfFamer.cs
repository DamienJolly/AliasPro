namespace AliasPro.API.Landing.Models
{
    public interface IHallOfFamer
    {
        uint Id { get; set; }
        int Amount { get; set; }
        string Username { get; set; }
        string Figure { get; set; }
    }
}
