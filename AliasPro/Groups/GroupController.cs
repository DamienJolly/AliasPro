using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.Groups.Models;
using AliasPro.Groups.Types;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Groups
{
    internal class GroupController : IGroupController
	{
		private readonly GroupDao _groupDao;

		private IDictionary<int, IGroup> _groups;
		private IList<IGroupBadgePart> _badgeParts;

		public GroupController(
			GroupDao groupDao)
        {
			_groupDao = groupDao;

			_groups = new Dictionary<int, IGroup>();
			_badgeParts = new List<IGroupBadgePart>();

			InitializeGroups();
		}

		public async void InitializeGroups()
		{
			_badgeParts = await _groupDao.ReadBadgeParts();
		}

		public async Task<IGroup> ReadGroupData(int groupId)
		{
			if (_groups.TryGetValue(groupId, out IGroup group))
				return group;

			group = await _groupDao.ReadGroupData(groupId);
			_groups.TryAdd(group.Id, group);

			return group;
		}

		public async Task<IGroup> CreateGroup(string name, string desc, uint playerId, int roomId, string badge, int colourOne, int colourTwo)
		{
			return await ReadGroupData(
				await _groupDao.CreateGroup(name, desc, playerId, roomId, badge, colourOne, colourTwo));
		}

		public async Task RemoveGroup(int groupId)
		{
			_groups.Remove(groupId);
			await _groupDao.RemoveGroup(groupId);
		}

		public async Task UpdateGroupMember(IGroup group, IGroupMember member)
		{
			//todo: remove
			await _groupDao.UpdateGroupMember(group.Id, member);
		}

		public async Task AddGroupMember(int groupId, IGroupMember member) =>
			await _groupDao.AddGroupMember(groupId, member);

		public async Task RemoveGroupMember(int groupId, int playerId) =>
			await _groupDao.RemoveGroupMember(groupId, playerId);

		public ICollection<IGroupBadgePart> GetBases => 
			_badgeParts.Where(part => part.Type == BadgePartType.BASE).ToList();

		public ICollection<IGroupBadgePart> GetSymbols => 
			_badgeParts.Where(part => part.Type == BadgePartType.SYMBOL).ToList();

		public ICollection<IGroupBadgePart> GetBaseColours => 
			_badgeParts.Where(part => part.Type == BadgePartType.BASE_COLOUR).ToList();

		public ICollection<IGroupBadgePart> GetSymbolColours => 
			_badgeParts.Where(part => part.Type == BadgePartType.SYMBOL_COLOUR).ToList();

		public ICollection<IGroupBadgePart> GetBackgroundColours => 
			_badgeParts.Where(part => part.Type == BadgePartType.BACKGROUND_COLOUR).ToList();
	}
}
