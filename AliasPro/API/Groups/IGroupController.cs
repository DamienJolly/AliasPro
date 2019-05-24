using AliasPro.API.Groups.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.API.Groups
{
    public interface IGroupController
	{
		void InitializeGroups();
		Task<IGroup> ReadGroupData(int groupId);
		ICollection<IGroupBadgePart> GetBases { get; }
		ICollection<IGroupBadgePart> GetSymbols { get; }
		ICollection<IGroupBadgePart> GetBaseColours { get; }
		ICollection<IGroupBadgePart> GetSymbolColours { get; }
		ICollection<IGroupBadgePart> GetBackgroundColours { get; }
	}
}
