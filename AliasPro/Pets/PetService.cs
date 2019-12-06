using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Pets;
using AliasPro.Pets.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Pets
{
    internal class PetService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PetDao>();
            collection.AddSingleton<IPetController, PetController>();

			AddPackets(collection);
		}

		private static void AddPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IAsyncPacket, RequestPetBreedsEvent>();
		}
	}
}
