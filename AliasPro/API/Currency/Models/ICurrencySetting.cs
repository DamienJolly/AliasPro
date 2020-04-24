namespace AliasPro.API.Currency.Models
{
    public interface ICurrencySetting
    {
        int Id { get; set; }
        int Time { get; set; }
        int Amount { get; set; }
        int Maximum { get; set; }
        int CyclesPerDay { get; set; }
    }
}
