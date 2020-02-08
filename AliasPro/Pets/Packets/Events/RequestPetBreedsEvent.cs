using AliasPro.API.Pets;
using AliasPro.API.Pets.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Pets.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Pets.Packets.Events
{
    public class RequestPetBreedsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestPetBreedsMessageEvent;

        private readonly IPetController _petController;

        public RequestPetBreedsEvent(IPetController petController)
        {
			_petController = petController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
			string name = clientPacket.ReadString();

			if (!_petController.TryGetPetData(name, out IPetData pet))
				return;

			await session.SendPacketAsync(new PetBreedsComposer(name, pet));
        }
    }
}
