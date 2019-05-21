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
			collection.AddSingleton<IAsyncPacket, ModerationMessageEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationAlertEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationTradeLockEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationMuteEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationPickTicketEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationReleaseTicketEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationCloseTicketEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationKickEvent>();
			collection.AddSingleton<IAsyncPacket, ModerationRequestRoomChatlogEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestIssueChatlogEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestRoomInfoEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRoomAlertEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationChangeRoomSettingsEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestUserInfoEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestRoomVisitsEvent>();
            collection.AddSingleton<IAsyncPacket, ModerationRequestUserChatlogEvent>();
        }
    }
}
