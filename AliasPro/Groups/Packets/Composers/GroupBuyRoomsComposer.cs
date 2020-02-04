using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupBuyRoomsComposer : IMessageComposer
	{
		private readonly ICollection<IRoomData> _rooms;

		public GroupBuyRoomsComposer(
			ICollection<IRoomData> rooms)
		{
			_rooms = rooms;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupBuyRoomsMessageComposer);
			message.WriteInt(10); // price

			message.WriteInt(_rooms.Count);
			foreach (IRoomData room in _rooms)
			{
				message.WriteInt((int)room.Id);
				message.WriteString(room.Name);
				message.WriteBoolean(false);
			}

			message.WriteInt(5);

			message.WriteInt(10);
			message.WriteInt(3);
			message.WriteInt(4);

			message.WriteInt(25);
			message.WriteInt(17);
			message.WriteInt(5);

			message.WriteInt(25);
			message.WriteInt(17);
			message.WriteInt(3);

			message.WriteInt(29);
			message.WriteInt(11);
			message.WriteInt(4);

			message.WriteInt(0);
			message.WriteInt(0);
			message.WriteInt(0);
			return message;
		}
	}
}
