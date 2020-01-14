using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.Groups.Imager;
using AliasPro.Groups.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Groups
{
    internal class GroupController : IGroupController
	{
		private readonly GroupDao _groupDao;

		private readonly IDictionary<int, IGroup> _groups;
		private IList<IGroupBadgePart> _badgeParts;
		public BadgeImager BadgeImager { get; set; }

		public GroupController(
			GroupDao groupDao)
        {
			_groupDao = groupDao;

			_groups = new Dictionary<int, IGroup>();
			_badgeParts = new List<IGroupBadgePart>();

			BadgeImager = new BadgeImager();

			InitializeGroups();
		}

		public async void InitializeGroups()
		{
			_badgeParts = await _groupDao.ReadBadgeParts();
		}

		public void InitalizeBadgeImager()
		{
			BadgeImager.Initialize(_badgeParts);
		}

		public async void Cycle()
		{
			foreach (IGroup group in _groups.Values.ToList())
			{
				group.IdleTime++;
				if (group.IdleTime >= 120)
				{
					await UpdateGroup(group);
					_groups.Remove(group.Id);
				}
			}
		}

		public async Task<IGroup> ReadGroupData(int groupId)
		{
			if (!_groups.TryGetValue(groupId, out IGroup group))
			{
				group = await _groupDao.ReadGroupData(groupId);
				if (group == null)
					return null;

				_groups.TryAdd(group.Id, group);
			}

			group.IdleTime = 0;
			return group;
		}

		public async Task<int> CreateGroup(IGroup group) => 
			await _groupDao.CreateGroup(group);

		public async Task UpdateGroup(IGroup group) =>
			await _groupDao.UpdateGroup(group);

		public bool TryAddGroup(IGroup group) =>
			_groups.TryAdd(group.Id, group);

		public async Task RemoveGroup(int groupId)
		{
			_groups.Remove(groupId);
			await _groupDao.RemoveGroup(groupId);
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
