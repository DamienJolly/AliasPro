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
    public class RoomUserPickupPetEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserPickupPetMessageEvent;

		private readonly IRoomController _roomController;

		public RoomUserPickupPetEvent(IRoomController roomController)
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
			if (!room.Entities.TryGetEntityById(botId, out BaseEntity entity))
				return;

			if (!(entity is PetEntity petEntity))
				return;

			if (petEntity.OwnerId != session.Player.Id)
				return;

			IPlayerPet playerPet = new PlayerPet(
				petEntity.Id,
				petEntity.Name,
				petEntity.Type,
				petEntity.Race,
				petEntity.Colour
			);

			if (!session.Player.Inventory.TryAddPet(playerPet))
				return;

			await room.RemoveEntity(petEntity, false);
			await _roomController.UpdateBotSettings(petEntity, 0);
			await session.SendPacketAsync(new AddPlayerItemsComposer(3, petEntity.PetId));
			await session.SendPacketAsync(new AddPetCompoer(playerPet));
		}
    }
}
