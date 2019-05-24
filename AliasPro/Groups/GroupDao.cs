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

		internal async Task<int> CreateGroup(string name, string desc, uint playerId, int roomId, string badge, int colourOne, int colourTwo)
		{
			int groupId = -1;
			await CreateTransaction(async transaction =>
			{
				groupId = await Insert(transaction, "INSERT INTO `groups` (`name`, `desc`, `owner_id`, `room_id`, `created_at`, `badge`, `colour1`, `colour2`) VALUES (@0, @1, @2, @3, UNIX_TIMESTAMP(), @4, @5, @6);",
					name, desc, playerId, roomId, badge, colourOne, colourTwo);

				await Insert(transaction, "UPDATE `rooms` SET `group_id` = @0 WHERE `id` = @1;",
					groupId, roomId);
			});
			return groupId;
		}
	}
}
