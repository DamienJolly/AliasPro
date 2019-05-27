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
						group.Members = await ReadGroupMembers(group.Id);
					}
				}, "SELECT * FROM `groups` WHERE `id` = @0 LIMIT 1;", groupId);
			});
			return group;
		}

		public async Task<IDictionary<int, IGroupMember>> ReadGroupMembers(int groupId)
		{
			IDictionary<int, IGroupMember> members = new Dictionary<int, IGroupMember>();
			await CreateTransaction(async transaction =>
			{
				await Select(transaction, async reader =>
				{
					while (await reader.ReadAsync())
					{
						IGroupMember member = new GroupMember(reader);

						if (!members.ContainsKey(member.PlayerId))
							members.Add(member.PlayerId, member);
					}
				}, "SELECT `group_members`.* , `players`.`username`, `players`.`figure` FROM `group_members` " +
					"INNER JOIN `players` ON `players`.`id` = `group_members`.`player_id` WHERE `group_members`.`group_id` = @0;", groupId);
			});
			return members;
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

		public async Task AddGroupMember(int groupId, IGroupMember member)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `group_members` (`player_id`, `group_id`, `join_date`, `rank`) VALUES (@1, @0, @3, @2);",
					groupId, member.PlayerId, (int)member.Rank, member.JoinData);
			});
		}

		internal async Task UpdateGroupMember(int groupId, IGroupMember member)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `group_members` SET `rank` = @1 WHERE `group_id` = @0 AND `player_id` = @2;",
					groupId, (int)member.Rank, member.PlayerId);
			});
		}

		internal async Task RemoveGroupMember(int groupId, int playerId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "DELETE FROM `group_members` WHERE `group_id` = @0 AND `player_id` = @1;",
					groupId, playerId);
			});
		}
	}
}
