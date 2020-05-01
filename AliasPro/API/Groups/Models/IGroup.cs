using AliasPro.Groups.Types;
using System.Collections.Generic;

namespace AliasPro.API.Groups.Models
{
	public interface IGroup
	{
		bool TryGetMember(int playerId, out IGroupMember member);
		bool TryAddMember(IGroupMember member);
		bool RemoveMember(int playerId);
		bool IsOwner(int playerId);
		bool IsAdmin(int playerId);
		bool IsMember(int playerId);
		int GetMembers { get; }
		int GetRequests { get; }

		int Id { get; set; }
		string Name { get; set; }
		string Description { get; set; }
		int OwnerId { get; set; }
		string OwnerName { get; set; }
		int CreatedAt { get; set; }
		int RoomId { get; set; }
		string RoomName { get; set; }
		GroupState State { get; set; }
		bool Rights { get; set; }
		string Badge { get; set; }
		int ColourOne { get; set; }
		int ColourTwo { get; set; }
		IDictionary<int, IGroupMember> Members { get; set; }

		int IdleTime { get; set; }
	}
}
