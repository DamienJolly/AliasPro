using AliasPro.Communication.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Game.Habbos
{
	public class HabboService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<HabboController>();
            collection.AddSingleton<HabboDao>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {

        }
    }
}
