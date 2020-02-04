using AliasPro.API.Pets;
using AliasPro.Communication.Messages;
using AliasPro.Pets.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Pets
{
    internal class PetService : IService
	{
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<PetDao>();
            collection.AddSingleton<IPetController, PetController>();

			RegisterPackets(collection);
		}

		private static void RegisterPackets(IServiceCollection collection)
		{
			collection.AddSingleton<IMessageEvent, RequestPetBreedsEvent>();
			collection.AddSingleton<IMessageEvent, CheckPetNameEvent>();
		}
	}
}
