using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Entities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomUserPlaceBotEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserPlaceBotMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserPlaceBotEvent(IRoomController roomController)
        {
			_roomController = roomController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			int botId = message.ReadInt();
			if (!session.Player.Inventory.TryGetBot(botId, out IPlayerBot bot))
				return;

			int posX = message.ReadInt();
			int posY = message.ReadInt();

			if (!room.RoomGrid.TryGetRoomTile(posX, posY, out IRoomTile tile))
				return;

			if (!tile.IsValidTile(null, true))
				return;

			int entityId = room.Entities.NextEntitityId++;
			BaseEntity botEntity = new BotEntity(
				bot.Id,
				session.Player.Id,
				session.Player.Username,
				entityId,
				posX,
				posY,
				4,
				room,
				bot.Name,
				bot.Figure,
				bot.Gender,
				bot.Motto,
				0,
				0,
				true);

			session.Player.Inventory.RemoveBot(bot.Id);

			await room.AddEntity(botEntity);
			await _roomController.UpdateBotSettings(botEntity, room.Id);
			await session.SendPacketAsync(new InventoryBotsComposer(session.Player.Inventory.Bots));
		}
    }
}
