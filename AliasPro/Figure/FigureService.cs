using AliasPro.API.Figure;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Figure.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Figure
{
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