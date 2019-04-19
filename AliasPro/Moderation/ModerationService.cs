using AliasPro.API.Moderation;
using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.Moderation.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Moderation
{
    internal class ModerationService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<ModerationDao>();
            collection.AddSingleton<IModerationController, ModerationController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, ModerationPickTicketEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationReleaseTicketEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationCloseTicketEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestRoomChatlogEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestIssueChatlogEvent>();
        }
    }
}
