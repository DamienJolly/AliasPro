using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupManageComposer : IPacketComposer
	{
		private readonly IGroup _group;

		public GroupManageComposer(
			IGroup group)
		{
			_group = group;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupManageMessageComposer);
			message.WriteInt(0);
			message.WriteBoolean(true);
			message.WriteInt(_group.Id);
			message.WriteString(_group.Name);
			message.WriteString(_group.Description);
			message.WriteInt(_group.RoomId);
			message.WriteInt(_group.ColourOne);
			message.WriteInt(_group.ColourTwo);
			message.WriteInt(0);
			message.WriteInt(0);
			message.WriteBoolean(false);
			message.WriteString("");
			message.WriteInt(5);

			string badge = _group.Badge;
			badge = badge.Replace("b", "");
			string[] data = badge.Split("s");

			foreach (string part in data)
			{
				message.WriteInt(part.Length >= 6 ? int.Parse(part.Substring(0, 3)) : int.Parse(part.Substring(0, 2)));
				message.WriteInt(part.Length >= 6 ? int.Parse(part.Substring(3, 2)) : int.Parse(part.Substring(2, 2)));
				message.WriteInt(part.Length < 5 ? 0 : part.Length >= 6 ? int.Parse(part.Substring(5, 1)) : int.Parse(part.Substring(4, 1)));
			}

			int i = 0;
			while (i < (5 - data.Length))
			{
				message.WriteInt(0);
				message.WriteInt(0);
				message.WriteInt(0);
				i++;
			}

			message.WriteString(_group.Badge);
			message.WriteInt(_group.GetMembers);
			return message;
		}
	}
}
