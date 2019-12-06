using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Pets;
using AliasPro.API.Pets.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Pets.Packets.Composers;

namespace AliasPro.Pets.Packets.Events
{
    public class RequestPetBreedsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestPetBreedsMessageEvent;

        private readonly IPetController _petController;

        public RequestPetBreedsEvent(IPetController petController)
        {
			_petController = petController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			string name = clientPacket.ReadString();

			if (!_petController.TryGetPetData(name, out IPetData pet))
				return;

			await session.SendPacketAsync(new PetBreedsComposer(name, pet));
        }
    }
}
