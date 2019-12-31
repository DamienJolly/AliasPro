using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestPetInformationEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestPetInformationMessageEvent;

		private readonly IRoomController _roomController;

		public RequestPetInformationEvent(IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			int petId = clientPacket.ReadInt();
			if (!room.Entities.TryGetEntityById(petId, out BaseEntity entity))
				return;

			if (!(entity is PetEntity petEntity))
				return;

			await session.SendPacketAsync(new PetInformationComposer(petEntity));
		}
    }
}
