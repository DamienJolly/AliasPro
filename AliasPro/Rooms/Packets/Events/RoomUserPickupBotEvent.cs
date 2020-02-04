using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Players.Models;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Entities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserPickupBotEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RoomUserPickupBotMessageEvent;

		private readonly IRoomController _roomController;

		public RoomUserPickupBotEvent(IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) 
				return;

			if (room.OwnerId != session.Player.Id)
				return;

			int botId = clientPacket.ReadInt();
			if (!room.Entities.TryGetEntityById(botId, out BaseEntity entity))
				return;

			if (!(entity is BotEntity botEntity))
				return;

			IPlayerBot playerBot = new PlayerBot(
				botEntity.BotId, 
				botEntity.Name, 
				botEntity.Motto, 
				botEntity.Gender, 
				botEntity.Figure
			);

			if (!session.Player.Inventory.TryAddBot(playerBot))
				return;

			await room.RemoveEntity(botEntity, false);
			await _roomController.UpdateBotSettings(botEntity, 0);
			await session.SendPacketAsync(new AddPlayerItemsComposer(5, botEntity.BotId));
			await session.SendPacketAsync(new AddBotCompoer(playerBot));
		}
    }
}
