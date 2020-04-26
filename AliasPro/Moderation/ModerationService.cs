using AliasPro.API.Moderation;
using AliasPro.Communication.Messages;
using AliasPro.Moderation.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Moderation
{
    internal class ModerationService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<ModerationDao>();
            collection.AddSingleton<IModerationController, ModerationController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
			collection.AddSingleton<IMessageEvent, ModerationMessageEvent>();
			collection.AddSingleton<IMessageEvent, ModerationAlertEvent>();
			collection.AddSingleton<IMessageEvent, ModerationTradeLockEvent>();
			collection.AddSingleton<IMessageEvent, ModerationMuteEvent>();
			collection.AddSingleton<IMessageEvent, ModerationBanEvent>();
			collection.AddSingleton<IMessageEvent, ModerationPickTicketEvent>();
            collection.AddSingleton<IMessageEvent, ModerationReleaseTicketEvent>();
            collection.AddSingleton<IMessageEvent, ModerationCloseTicketEvent>();
			collection.AddSingleton<IMessageEvent, ModerationKickEvent>();
			collection.AddSingleton<IMessageEvent, ModerationRequestRoomChatlogEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRequestIssueChatlogEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRequestRoomInfoEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRoomAlertEvent>();
            collection.AddSingleton<IMessageEvent, ModerationChangeRoomSettingsEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRequestUserInfoEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRequestRoomVisitsEvent>();
            collection.AddSingleton<IMessageEvent, ModerationRequestUserChatlogEvent>();
            collection.AddSingleton<IMessageEvent, ModerationReportEvent>();
        }
    }
}
