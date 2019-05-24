using AliasPro.Groups.Types;

namespace AliasPro.API.Groups.Models
{
	public interface IGroupBadgePart
	{
		int Id { get; set; }
		string AssetOne { get; set; }
		string AssetTwo { get; set; }
		BadgePartType Type { get; set; }
	}
}
