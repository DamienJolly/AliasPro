using AliasPro.API.Groups.Models;
using AliasPro.Groups.Imager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Groups
{
    public interface IGroupController
	{
		BadgeImager BadgeImager { get; set; }
		void InitializeGroups();
		void Cycle();
		Task<IGroup> ReadGroupData(int groupId);
		Task<int> CreateGroup(IGroup group);
		Task UpdateGroup(IGroup group);
		bool TryAddGroup(IGroup group);
		Task RemoveGroup(int groupId);
		Task AddGroupMember(int groupId, IGroupMember member);
		Task RemoveGroupMember(int groupId, int playerId);
		void InitalizeBadgeImager();

		ICollection<IGroupBadgePart> GetBases { get; }
		ICollection<IGroupBadgePart> GetSymbols { get; }
		ICollection<IGroupBadgePart> GetBaseColours { get; }
		ICollection<IGroupBadgePart> GetSymbolColours { get; }
		ICollection<IGroupBadgePart> GetBackgroundColours { get; }
	}
}
