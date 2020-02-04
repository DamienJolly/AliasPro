using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Players.Models
{
    public interface IPlayerPet
	{
		void Serialize(ServerMessage message);

		int Id { get; set; }
		string Name { get; set; }
		int Type { get; set; }
		int Race { get; set; }
		string Colour { get; set; }
		int Experience { get; set; }
		int Happyness { get; set; }
		int Energy { get; set; }
		int Hunger { get; set; }
		int Thirst { get; set; }
		int Respect { get; set; }
		int Created { get; set; }
	}
}
