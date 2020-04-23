using AliasPro.API.Badge;
using AliasPro.API.Badges.Models;
using AliasPro.API.Permissions;
using AliasPro.API.Players.Models;
using AliasPro.Badges.Packets.Composers;
using AliasPro.Players.Models;
using System.Collections.Generic;

namespace AliasPro.Badges
{
	internal class BadgeController : IBadgeController
	{
		private readonly BadgeDao _badgeDao;

		private IDictionary<string, IBadge> _badges;

		public BadgeController(BadgeDao badgeDao)
		{
			_badgeDao = badgeDao;

			_badges = new Dictionary<string, IBadge>();

			InitializeBadges();
		}

		public async void InitializeBadges()
		{
			_badges = await _badgeDao.ReadBadges();
		}

		public ICollection<IBadge> Badges =>
			_badges.Values;

		public bool TryGetBadge(string code, out IBadge badge) =>
			_badges.TryGetValue(code, out badge);

		public async void AddPlayerBadge(IPlayer player, string code)
		{
			if (player.Badge.TryGetBadge(code, out _))
				return;

			if (!TryGetBadge(code, out IBadge badge))
				return;

			if (!string.IsNullOrEmpty(badge.RequiredRight))
			{
				if (!Program.GetService<IPermissionsController>().HasPermission(player, badge.RequiredRight))
					return;
			}

			IPlayerBadge playerBadge = new PlayerBadge(badge.Id, badge.Code);
			player.Badge.AddBadge(playerBadge);
			await _badgeDao.AddPlayerBadge(player.Id, playerBadge);

			if (player.Session != null)
				await player.Session.SendPacketAsync(new AddPlayerBadgeComposer(playerBadge));
		}

		public async void RemovePlayerBadge(IPlayer player, IPlayerBadge badge)
		{
			player.Badge.Badges.Remove(badge);
			await _badgeDao.RemovePlayerBadge(player.Id, badge.Code);
		}

		public async void UpdatePlayerBadge(IPlayer player, IPlayerBadge badge, string code)
		{
			await _badgeDao.UpdatePlayerBadge(player.Id, badge.Code, code);
			badge.Code = code;
		}
	}
}
