using AliasPro.API.Players.Models;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Players.Components
{
    public class BadgeComponent
    {
        private readonly IDictionary<string, IPlayerBadge> _badges;

        public BadgeComponent(
            IDictionary<string, IPlayerBadge> badges)
        {
            _badges = badges;
        }

        public void ResetBadges()
        {
            foreach (IPlayerBadge badge in WornBadges)
            {
                badge.Slot = 0;
            }
        }

        public bool TryGetBadge(string code, out IPlayerBadge badge) =>
            _badges.TryGetValue(code, out badge);

		public bool HasBadge(string code) =>
			_badges.ContainsKey(code);

		public void AddBadge(IPlayerBadge badge) =>
			_badges.Add(badge.Code, badge);

		public ICollection<IPlayerBadge> WornBadges =>
            _badges.Values.Where(badge => badge.Slot > 0).OrderBy(badge => badge.Slot).ToList();

        public ICollection<IPlayerBadge> Badges =>
            _badges.Values;
    }
}
