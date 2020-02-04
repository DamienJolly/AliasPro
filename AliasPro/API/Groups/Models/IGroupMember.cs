using AliasPro.API.Groups.Types;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Groups.Models
{
	public interface IGroupMember
	{
		void Compose(ServerMessage message);

		int PlayerId { get; set; }
		string Username { get; set; }
		string Figure { get; set; }
		int JoinData { get; set; }
		GroupRank Rank { get; set; }
	}
}
