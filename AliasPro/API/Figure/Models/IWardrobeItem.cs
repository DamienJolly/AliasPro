using AliasPro.Players.Types;

namespace AliasPro.API.Figure.Models
{
	public interface IWardrobeItem
	{
		int SlotId { get; set; }
		string Figure { get; set; }
		PlayerGender Gender { get; set; }
	}
}
