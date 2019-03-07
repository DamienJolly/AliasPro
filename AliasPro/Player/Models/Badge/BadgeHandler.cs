using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Player.Models.Badge
{
    public class BadgeHandler
    {
        private readonly IDictionary<string, IBadgeData> _badges;

        internal BadgeHandler(IDictionary<string, IBadgeData> badges)
        {
            _badges = badges;
        }

        public void ResetBadges()
        {
            foreach (IBadgeData badge in WearableBadges)
            {
                badge.Slot = 0;
            }
        }

        public bool TryGetBadge(string code, out IBadgeData badge) =>
            _badges.TryGetValue(code, out badge);

        public ICollection<IBadgeData> WearableBadges => 
            _badges.Values.Where(badge => badge.Slot > 0).OrderBy(badge => badge.Slot).ToList();

        public ICollection<IBadgeData> Badges =>
            _badges.Values;
    }
}
