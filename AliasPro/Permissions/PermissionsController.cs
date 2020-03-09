using AliasPro.API.Permissions;
using AliasPro.API.Permissions.Models;
using AliasPro.API.Players.Models;
using System.Collections.Generic;

namespace AliasPro.Permissions
{
    internal class PermissionsController : IPermissionsController
	{
		private readonly PermissionsDao _permissionsDao;

		private IDictionary<int, IPermission> _rankPerms;
		private IDictionary<int, IPermission> _subPerms;

		public PermissionsController(PermissionsDao permissionsDao)
        {
			_permissionsDao = permissionsDao;

			_rankPerms = new Dictionary<int, IPermission>();
			_subPerms = new Dictionary<int, IPermission>();

			InitializePermissions();
		}

		public async void InitializePermissions()
		{
			_rankPerms = await _permissionsDao.ReadRankPermissions();
			_subPerms = await _permissionsDao.ReadSubPermissions();
		}

		public bool HasPermission(IPlayer player, string param)
		{
			if (_rankPerms.TryGetValue(player.Rank, out IPermission rankPerm))
				return rankPerm.HasPermission(param);

			if (_subPerms.TryGetValue(player.VipRank, out IPermission subPerm))
				return subPerm.HasPermission(param);

			return false;
		}
	}
}
