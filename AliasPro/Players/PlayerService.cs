using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Players;
using AliasPro.Players.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Players
{
    internal class PlayerService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<PlayerDao>();
            collection.AddSingleton<PlayerRepostiory>();
            collection.AddSingleton<IPlayerController, PlayerController>();

            AddPackets(collection);
        }

        private static void AddPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IAsyncPacket, UniqueIdEvent>();
            collection.AddSingleton<IAsyncPacket, SecureLoginEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserDataEvent>();
            collection.AddSingleton<IAsyncPacket, RequestFurniInventoryEvent>();
			collection.AddSingleton<IAsyncPacket, RequestBotInventoryEvent>();
			collection.AddSingleton<IAsyncPacket, RequestPetInventoryEvent>();
			collection.AddSingleton<IAsyncPacket, RequestBadgeInventoryEvent>();
            collection.AddSingleton<IAsyncPacket, RequestWearingBadgesEvent>();
            collection.AddSingleton<IAsyncPacket, UserWearBadgeEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserCreditsEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserProfileEvent>();
			collection.AddSingleton<IAsyncPacket, RequestUserClubEvent>();
			collection.AddSingleton<IAsyncPacket, RequestProfileFriendsEvent>();
            collection.AddSingleton<IAsyncPacket, SaveUserVolumesEvent>();
            collection.AddSingleton<IAsyncPacket, SavePreferOldChatEvent>();
            collection.AddSingleton<IAsyncPacket, SaveIgnoreRoomInvitesEvent>();
            collection.AddSingleton<IAsyncPacket, SaveBlockCameraFollowEvent>();
            collection.AddSingleton<IAsyncPacket, RequestMeMenuSettingsEvent>();
            collection.AddSingleton<IAsyncPacket, UserChangeMottoEvent>();
            collection.AddSingleton<IAsyncPacket, RequestUserIgnoresEvent>();
        }
    }
}
