namespace AliasPro.API.Players.Models
{
    public interface IPlayerPet
	{
		int Id { get; set; }
		string Name { get; set; }
		int Type { get; set; }
		int Race { get; set; }
		string Colour { get; set; }
	}
}
