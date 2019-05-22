using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Permissions.Models;
using AliasPro.Permissions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Permissions
{
    internal class PermissionsDao : BaseDao
    {
        public PermissionsDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

		public async Task<IDictionary<int, IPermission>> ReadRankPermissions()
		{
			IDictionary<int, IPermission> permissions = new Dictionary<int, IPermission>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPermission permission = new Permission(reader);
						permission.Permissions = 
							await ReadPermissions(permission.Level, 0);

						if (!permissions.ContainsKey(permission.Level))
							permissions.Add(permission.Level, permission);
					}
				}, "SELECT * FROM `permission_ranks`;");
			});
			return permissions;
		}

		public async Task<IDictionary<int, IPermission>> ReadSubPermissions()
		{
			IDictionary<int, IPermission> permissions = new Dictionary<int, IPermission>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IPermission permission = new Permission(reader);
						permission.Permissions =
							await ReadPermissions(0, permission.Level);

						if (!permissions.ContainsKey(permission.Level))
							permissions.Add(permission.Level, permission);
					}
				}, "SELECT * FROM `permission_subscriptions`;");
			});
			return permissions;
		}

		private async Task<IList<string>> ReadPermissions(int rankLevel, int subLevel)
		{
			IList<string> permissions = new List<string>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						permissions.Add(reader.ReadData<string>("permission"));
					}
				}, "SELECT * FROM `permissions` WHERE `rank_level` = @0 AND `sub_level` = @1;", rankLevel, subLevel);
			});
			return permissions;
		}
	}
}
