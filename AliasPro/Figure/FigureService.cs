using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Figure
{
    using Network;
    using Network.Events;
    using Packets.Incoming;

    internal class FigureService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<FigureDao>();
            collection.AddSingleton<FigureRepostiory>();
            collection.AddSingleton<IFigureController, FigureController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, UpdateFigureEvent>();
        }
    }
}