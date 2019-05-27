using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupMembersComposer : IPacketComposer
	{
		private readonly IPlayer _player;
		private readonly IGroup _group;
		private readonly int _pageId;
		private readonly int _levelId;
		private readonly string _query;

		public GroupMembersComposer(
			IPlayer player, IGroup group,
			int pageId, int levelId, string query)
		{
			_player = player;
			_group = group;
			_pageId = pageId;
			_levelId = levelId;
			_query = query;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupMembersMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteString(_group.Name);
			message.WriteInt(_group.RoomId);
			message.WriteString(_group.Badge);

			ICollection<IGroupMember> members = SearchMembers(
				_group.Members.Values, _pageId, _levelId, _query);

			message.WriteInt(members.Count);
			message.WriteInt(members.Count);
			foreach (IGroupMember member in members)
				member.Compose(message);

			message.WriteBoolean(_group.IsOwner((int)_player.Id));
			message.WriteInt(14);
			message.WriteInt(_pageId);
			message.WriteInt(_levelId);
			message.WriteString(_query);
			return message;
		}

		private ICollection<IGroupMember> SearchMembers(ICollection<IGroupMember> members, int page, int levelId, string query)
		{
			List<IGroupMember> membersToGo = new List<IGroupMember>();
			switch (levelId)
			{
				case 2: membersToGo = members.Where(member => (int)member.Rank == 3 && member.Username.Contains(query)).ToList(); break;
				case 1: membersToGo = members.Where(member => (int)member.Rank <= 1 && member.Username.Contains(query)).ToList(); break;
				default: membersToGo = members.Where(member => (int)member.Rank <= 2 && member.Username.Contains(query)).ToList(); break;
			}

			while (page * 14 > membersToGo.Count) page--;

			return membersToGo.GetRange(page * 14, (page * 14) + 14 > membersToGo.Count ? membersToGo.Count - page * 14 : (page * 14) + 14);
		}
	}
}
