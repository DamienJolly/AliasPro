using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Types;

namespace AliasPro.API.Rooms.Games.Models
{
	public interface IGamePlayer
	{
		PlayerEntity Entity { get; set; }
		int Points { get; set; }
		IGameTeam Team { get; set; }
		BaseGame Game { get; set; }
	}
}
