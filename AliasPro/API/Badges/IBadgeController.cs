using AliasPro.API.Badges.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.API.Badge
{
    public interface IBadgeController
	{
		void InitializeBadges();
		ICollection<IBadge> Badges { get; }
		bool TryGetBadge(string code, out IBadge badge);
		void AddPlayerBadge(IPlayer player, string code);
		void RemovePlayerBadge(IPlayer player, IPlayerBadge badge);
		void UpdatePlayerBadge(IPlayer player, IPlayerBadge badge, string code);
	}
}
