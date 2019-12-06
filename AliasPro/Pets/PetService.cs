using AliasPro.API.Network;
using AliasPro.API.Pets;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Pets
{
    internal class PetService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PetDao>();
            collection.AddSingleton<IPetController, PetController>();
        }
    }
}
