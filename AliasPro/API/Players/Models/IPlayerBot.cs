using AliasPro.Players.Types;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerBot
	{
		int Id { get; set; }
		string Name { get; set; }
		string Motto { get; set; }
		PlayerGender Gender { get; set; }
		string Figure { get; set; }
	}
}
