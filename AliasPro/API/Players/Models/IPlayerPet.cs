using AliasPro.Players.Types;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerPet
	{
		int Id { get; set; }
		string Name { get; set; }
		string Motto { get; set; }
		PlayerGender Gender { get; set; }
		int Type { get; set; }
		int Race { get; set; }
		string Colour { get; set; }
	}
}
