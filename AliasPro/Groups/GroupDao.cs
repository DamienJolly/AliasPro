using AliasPro.API.Configuration;
using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.Groups.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Groups
{
    internal class GroupDao : BaseDao
    {
        public GroupDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }

		public async Task<IList<IGroupBadgePart>> ReadBadgeParts()
		{
			IList<IGroupBadgePart> badgeParts = new List<IGroupBadgePart>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						badgeParts.Add(new GroupBadgePart(reader));
					}
				}, "SELECT * FROM `group_badge_parts`;");
			});
			return badgeParts;
		}

		public async Task<IGroup> ReadGroupData(int groupId)
		{
			IGroup group = null;
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					if (await reader.ReadAsync())
					{
						group = new Group(reader);
					}
				}, "SELECT * FROM `groups` WHERE `id` = @0 LIMIT 1;", groupId);
			});
			return group;
		}
	}
}
