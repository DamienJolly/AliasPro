using AliasPro.API.Groups.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Groups
{
    public interface IGroupController
	{
		void InitializeGroups();
		void Cycle();
		Task<IGroup> ReadGroupData(int groupId);
		Task<IGroup> CreateGroup(string name, string desc, uint playerId, int roomId, string badge, int colourOne, int colourTwo);
		Task UpdateGroup(IGroup group);
		Task RemoveGroup(int groupId);
		Task AddGroupMember(int groupId, IGroupMember member);
		Task RemoveGroupMember(int groupId, int playerId);

		ICollection<IGroupBadgePart> GetBases { get; }
		ICollection<IGroupBadgePart> GetSymbols { get; }
		ICollection<IGroupBadgePart> GetBaseColours { get; }
		ICollection<IGroupBadgePart> GetSymbolColours { get; }
		ICollection<IGroupBadgePart> GetBackgroundColours { get; }
	}
}
