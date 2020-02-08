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
    public class RoomUserPlacePetEvent : IMessageEvent
    {
        public short Header => Incoming.RoomUserPlacePetMessageEvent;

        private readonly IRoomController _roomController;

        public RoomUserPlacePetEvent(IRoomController roomController)
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

			int petId = clientPacket.ReadInt();
			if (!session.Player.Inventory.TryGetPet(petId, out IPlayerPet pet))
				return;

			int posX = clientPacket.ReadInt();
			int posY = clientPacket.ReadInt();

			if (!room.RoomGrid.TryGetRoomTile(posX, posY, out IRoomTile tile))
				return;

			if (!tile.IsValidTile(null, true))
				return;

			int entityId = room.Entities.NextEntitityId++;
			BaseEntity petEntity = new PetEntity(
				pet.Id,
				pet.Type,
				pet.Race,
				pet.Colour,
				pet.Experience,
				pet.Happyness,
				pet.Energy,
				pet.Hunger,
				pet.Thirst,
				pet.Respect,
				pet.Created,
				session.Player.Id,
				session.Player.Username,
				entityId,
				posX,
				posY,
				4,
				room,
				pet.Name);

			session.Player.Inventory.RemovePet(pet.Id);

			await room.AddEntity(petEntity);
			await _roomController.UpdatePetSettings(petEntity, room.Id);
			await session.SendPacketAsync(new InventoryPetsComposer(session.Player.Inventory.Pets));
		}
    }
}
