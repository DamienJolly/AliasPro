using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupInfoComposer : IPacketComposer
	{
		private readonly IGroup _group;
		private readonly IPlayer _player;
		private readonly bool _newWindow;

		public GroupInfoComposer(
			IGroup group,
			IPlayer player,
			bool newWindow)
		{
			_group = group;
			_player = player;
			_newWindow = newWindow;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupInfoMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteBoolean(true); // ??
			message.WriteInt(0); // state
			message.WriteString(_group.Name);
			message.WriteString(_group.Description);
			message.WriteString(""); // badge
			message.WriteInt(_group.RoomId);
			message.WriteString("hello"); // room name
			message.WriteInt(3); // ?
			message.WriteInt(69); // memember count
			message.WriteBoolean(false); // ??
			message.WriteString(""); // created
			message.WriteBoolean(_group.OwnerId == _player.Id);
			message.WriteBoolean(true); // is admin
			message.WriteString("Damien"); // owner name
			message.WriteBoolean(_newWindow);
			message.WriteBoolean(false); // user can furni
			message.WriteInt(0); // invite count
			message.WriteBoolean(true); // forum
			return message;
		}
	}
}
