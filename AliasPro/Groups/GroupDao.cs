using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.Groups.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Groups
{
    internal class GroupDao : BaseDao
    {
        public GroupDao(ILogger<BaseDao> logger)
			: base(logger)
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
				}, "SELECT `groups`.*, `rooms`.`name` AS `room_name`, `players`.`username` AS `owner_name` FROM `groups` " +
					"INNER JOIN `rooms` ON `groups`.`room_id` = `rooms`.`id` " +
					"INNER JOIN `players` ON `groups`.`owner_id` = `players`.`id` WHERE `groups`.`id` = @0 LIMIT 1;", groupId);
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

		internal async Task<int> CreateGroup(IGroup group)
		{
			int groupId = -1;
			await CreateTransaction(async transaction =>
			{
				groupId = await Insert(transaction, "INSERT INTO `groups` (`name`, `desc`, `owner_id`, `room_id`, `created_at`, `badge`, `colour1`, `colour2`) VALUES (@0, @1, @2, @3, UNIX_TIMESTAMP(), @4, @5, @6);",
					group.Name, group.Description, group.OwnerId, group.RoomId, group.Badge, group.ColourOne, group.ColourTwo);

				await Insert(transaction, "UPDATE `rooms` SET `group_id` = @0 WHERE `id` = @1;",
					groupId, group.RoomId);
			});
			return groupId;
		}

		internal async Task UpdateGroup(IGroup group)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "UPDATE `groups` SET `name` = @1, `desc` = @2, `badge` = @3, `state` = @4, `colour1` = @5, `colour2` = @6, `rights` = @7 WHERE `id` = @0;",
					group.Id, group.Name, group.Description, group.Badge, (int)group.State, group.ColourOne, group.ColourTwo, group.Rights ? "1" : "0");

				foreach (IGroupMember member in group.Members.Values)
				{
					await Insert(transaction, "UPDATE `group_members` SET `rank` = @1 WHERE `group_id` = @0 AND `player_id` = @2;",
						group.Id, (int)member.Rank, member.PlayerId);
				}
			});
		}

		public async Task AddGroupMember(int groupId, IGroupMember member)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "INSERT INTO `group_members` (`player_id`, `group_id`, `join_date`, `rank`) VALUES (@1, @0, @3, @2);",
					groupId, member.PlayerId, (int)member.Rank, member.JoinData);
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

		internal async Task RemoveGroup(int groupId)
		{
			await CreateTransaction(async transaction =>
			{
				await Insert(transaction, "DELETE FROM `groups` WHERE `id` = @0;",
					groupId);

				await Insert(transaction, "DELETE FROM `group_members` WHERE `group_id` = @0;",
					groupId);
			});
		}
	}
}
