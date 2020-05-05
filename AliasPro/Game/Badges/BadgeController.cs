using AliasPro.Game.Badges.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AliasPro.Game.Badges
{
	internal class BadgeController
	{
		private readonly ILogger<BadgeController> logger;
		private readonly BadgeDao badgeDao;

		private Dictionary<string, BadgeData> badges;

		public BadgeController(
			ILogger<BadgeController> logger,
			BadgeDao badgeDao)
		{
			this.logger = logger;
			this.badgeDao = badgeDao;

			badges = new Dictionary<string, BadgeData>();

			InitializeBadges();

			this.logger.LogInformation("Loaded " + badges.Count + " badge definitions.");
		}

		public async void InitializeBadges()
		{
			badges = await badgeDao.ReadBadges();
		}

		public bool TryGetBadge(string code, out BadgeData badge) =>
			badges.TryGetValue(code, out badge);
	}
}
