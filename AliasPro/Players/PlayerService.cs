﻿using AliasPro.API.Players;
using AliasPro.Communication.Messages;
using AliasPro.Players.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Players
{
    internal class PlayerService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<PlayerDao>();
            collection.AddSingleton<PlayerRepostiory>();
            collection.AddSingleton<IPlayerController, PlayerController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, UniqueIdEvent>();
            collection.AddSingleton<IMessageEvent, SecureLoginEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserDataEvent>();
            collection.AddSingleton<IMessageEvent, RequestFurniInventoryEvent>();
			collection.AddSingleton<IMessageEvent, RequestBotInventoryEvent>();
			collection.AddSingleton<IMessageEvent, RequestPetInventoryEvent>();
			collection.AddSingleton<IMessageEvent, RequestBadgeInventoryEvent>();
            collection.AddSingleton<IMessageEvent, RequestWearingBadgesEvent>();
            collection.AddSingleton<IMessageEvent, UserWearBadgeEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserCreditsEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserProfileEvent>();
			collection.AddSingleton<IMessageEvent, RequestUserClubEvent>();
			collection.AddSingleton<IMessageEvent, RequestProfileFriendsEvent>();
            collection.AddSingleton<IMessageEvent, SaveUserVolumesEvent>();
            collection.AddSingleton<IMessageEvent, SavePreferOldChatEvent>();
            collection.AddSingleton<IMessageEvent, SaveIgnoreRoomInvitesEvent>();
            collection.AddSingleton<IMessageEvent, SaveBlockCameraFollowEvent>();
            collection.AddSingleton<IMessageEvent, RequestMeMenuSettingsEvent>();
            collection.AddSingleton<IMessageEvent, UserChangeMottoEvent>();
            collection.AddSingleton<IMessageEvent, RequestUserIgnoresEvent>();
        }
    }
}
