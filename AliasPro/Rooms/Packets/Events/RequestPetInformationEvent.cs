using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RequestPetInformationEvent : IMessageEvent
    {
        public short Header => Incoming.RequestPetInformationMessageEvent;

		private readonly IRoomController _roomController;

		public RequestPetInformationEvent(IRoomController roomController)
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
			if (!room.Entities.TryGetEntityById(petId, out BaseEntity entity))
				return;

			if (!(entity is PetEntity petEntity))
				return;

			await session.SendPacketAsync(new PetInformationComposer(petEntity));
		}
    }
}
