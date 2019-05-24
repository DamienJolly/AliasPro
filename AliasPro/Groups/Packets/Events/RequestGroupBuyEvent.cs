using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupBuyEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupBuyMessageEvent;

		private readonly IGroupController _groupController;
		private readonly IRoomController _roomController;

		public RequestGroupBuyEvent(
			IGroupController groupController,
			IRoomController roomController)
		{
			_groupController = groupController;
			_roomController = roomController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
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

			IRoomData room = 
				await _roomController.ReadRoomDataAsync((uint)roomId);
			if (room == null) return;

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

			//todo: server generate badge??

			IGroup group = await _groupController.CreateGroup(
				name, description, session.Player.Id, roomId, badge, colourOne, colourTwo);
			room.Group = group;

			await session.SendPacketAsync(new PurchaseOKComposer());
			await session.SendPacketAsync(new GroupBoughtComposer(group));
		}
	}
}

