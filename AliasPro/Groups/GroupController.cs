﻿using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.Groups.Types;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Groups
{
    internal class GroupController : IGroupController
	{
		private readonly GroupDao _groupDao;

		private IList<IGroupBadgePart> _badgeParts;

		public GroupController(
			GroupDao groupDao)
        {
			_groupDao = groupDao;

			_badgeParts = new List<IGroupBadgePart>();

			InitializeGroups();
		}

		public async void InitializeGroups()
		{
			_badgeParts = await _groupDao.ReadBadgeParts();
		}

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