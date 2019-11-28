using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.API.Figure.Models
{
	public interface IWardrobeItem
	{
		void Compose(ServerPacket message);

		int SlotId { get; set; }
		string Figure { get; set; }
		PlayerGender Gender { get; set; }
	}
}
