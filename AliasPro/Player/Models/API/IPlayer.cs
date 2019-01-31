namespace AliasPro.Player.Models
{
    public interface IPlayer
    {
        uint Id { get; set; }
        int Credits { get; set; }
        int Rank { get; set; }
        string Username { get; set; }
        string SsoTicket { get; set; }
        string Figure { get; set; }
        string Gender { get; set; }
        string Motto { get; set; }
        //todo: all other data
    }
}
