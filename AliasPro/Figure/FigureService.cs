using AliasPro.API.Figure;
using AliasPro.Communication.Messages;
using AliasPro.Figure.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Figure
{
    internal class FigureService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<FigureDao>();
            collection.AddSingleton<IFigureController, FigureController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, UpdateFigureEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserWardrobeEvent>();
            collection.AddSingleton<IMessageEvent, SaveWardrobeEvent>();
        }
    }
}