using AliasPro.API.Groups.Models;
using System.Collections.Generic;

namespace AliasPro.API.Groups
{
    public interface IGroupController
	{
		void InitializeGroups();
		ICollection<IGroupBadgePart> GetBases { get; }
		ICollection<IGroupBadgePart> GetSymbols { get; }
		ICollection<IGroupBadgePart> GetBaseColours { get; }
		ICollection<IGroupBadgePart> GetSymbolColours { get; }
		ICollection<IGroupBadgePart> GetBackgroundColours { get; }
	}
}
