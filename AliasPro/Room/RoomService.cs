using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Room
{
    using Network;
    using Network.Events;
    using Packets.Incoming;

    internal class RoomService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<RoomDao>();
            collection.AddSingleton<RoomRepository>();
            collection.AddSingleton<IRoomController, RoomController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, RequestRoomLoadEvent>();
            collection.AddSingleton<IAsyncPacket, RequestRoomEntryDataEvent>();
            collection.AddSingleton<IAsyncPacket, RequestFurnitureAliasesEvent>();
            collection.AddSingleton<IAsyncPacket, MoveAvatarEvent>();
            collection.AddSingleton<IAsyncPacket, AvatarChatEvent>();
            collection.AddSingleton<IAsyncPacket, RequestRoomDataEvent>();
        }
    }
}
