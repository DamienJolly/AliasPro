using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupBuyEvent : IMessageEvent
	{
		public short Id { get; } = Incoming.RequestGroupBuyMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public RequestGroupBuyEvent(
			IGroupController groupController,
			IRoomController roomController)
		{
			_groupController = groupController;
			_roomController = roomController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage clientPacket)
		{
			int guildPrice = 10;
			if (session.Player.Credits >= guildPrice)
			{
				session.Player.Credits -= guildPrice;
				await session.SendPacketAsync(new UserCreditsComposer(session.Player.Credits));
			}
			else
			{
				await session.SendPacketAsync(new AlertPurchaseFailedComposer(AlertPurchaseFailedComposer.SERVER_ERROR));
				return;
			}

			string name = clientPacket.ReadString();
			if (name.Length <= 0) return;

			string description = clientPacket.ReadString();
			int roomId = clientPacket.ReadInt();

			IRoom room = await _roomController.LoadRoom((uint)roomId);
			if (room == null)
				return;

			if (room.Group != null) return;

			if (room.OwnerId != session.Player.Id) return;

			int colourOne = clientPacket.ReadInt();
			int colourTwo = clientPacket.ReadInt();
			int count = clientPacket.ReadInt();

			string badge = "";
			for (int i = 0; i < count; i += 3)
			{
				int id = clientPacket.ReadInt();
				int colour = clientPacket.ReadInt();
				int pos = clientPacket.ReadInt();

				if (i == 0) badge += "b";
				else badge += "s";

				badge += (id < 100 ? "0" : "") + (id < 10 ? "0" : "") + id + (colour < 10 ? "0" : "") + colour + "" + pos;
			}

			IGroup group = new Group(
				name,
				description,
				(int)session.Player.Id,
				session.Player.Username,
				(int)room.Id,
				room.Name,
				badge, 
				colourOne, 
				colourTwo
			);

			IGroupMember member = new GroupMember(
				(int)session.Player.Id,
				session.Player.Username,
				session.Player.Figure,
				(int)UnixTimestamp.Now,
				GroupRank.OWNER
			);

			if (!group.TryAddMember(member)) return;

			group.Id = await _groupController.CreateGroup(group);
			await _groupController.AddGroupMember(group.Id, member);

			if (!_groupController.TryAddGroup(group)) return;

			room.Group = group;

			if (!session.Player.HasGroup(group.Id))
				session.Player.AddGroup(group.Id);

			_groupController.BadgeImager.GenerateImage(group.Badge);

			await session.SendPacketAsync(new PurchaseOKComposer());
			await session.SendPacketAsync(new GroupBoughtComposer(group));
		}
	}
}

