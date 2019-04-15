namespace AliasPro.API.Players.Models
{
    public interface IPlayerCurrency
    {
        int Type { get; }
        int Amount { get; set; }
    }
}
