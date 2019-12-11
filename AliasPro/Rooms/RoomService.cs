using AliasPro.API.Network;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms;
using AliasPro.Rooms.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Rooms
{
    internal class RoomService : INetworkService
    {
        public void SetupService(IServiceCollection collection)
        {
            collection.AddSingleton<RoomDao>();
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
            collection.AddSingleton<IAsyncPacket, RequestCreateRoomEvent>();
            collection.AddSingleton<IAsyncPacket, RequestRoomSettingsEvent>();
            collection.AddSingleton<IAsyncPacket, RoomSettingsSaveEvent>();
            collection.AddSingleton<IAsyncPacket, UserStartTypingEvent>();
            collection.AddSingleton<IAsyncPacket, UserStopTypingEvent>();
            collection.AddSingleton<IAsyncPacket, UserLookAtPointEvent>();
            collection.AddSingleton<IAsyncPacket, UserDanceEvent>();
            collection.AddSingleton<IAsyncPacket, UserActionEvent>();
            collection.AddSingleton<IAsyncPacket, UserSitEvent>();
            collection.AddSingleton<IAsyncPacket, UserRemoveRightsEvent>();
            collection.AddSingleton<IAsyncPacket, UserGiveRightsEvent>();
            collection.AddSingleton<IAsyncPacket, WiredTriggerSaveDataEvent>();
            collection.AddSingleton<IAsyncPacket, WiredEffectSaveDataEvent>();
            collection.AddSingleton<IAsyncPacket, WiredConditionSaveDataEvent>();
			collection.AddSingleton<IAsyncPacket, RoomUserPlaceBotEvent>();
			collection.AddSingleton<IAsyncPacket, RoomBotSettingsEvent>();
			collection.AddSingleton<IAsyncPacket, RoomBotSaveSettingsEvent>();
			collection.AddSingleton<IAsyncPacket, RoomUserPickupBotEvent>();
			collection.AddSingleton<IAsyncPacket, RoomUserPlacePetEvent>();
			collection.AddSingleton<IAsyncPacket, RoomUserPickupPetEvent>();
			collection.AddSingleton<IAsyncPacket, RequestPetInformationEvent>();
			collection.AddSingleton<IAsyncPacket, RequestFloorPlanBlockedTilesEvent>();
			collection.AddSingleton<IAsyncPacket, RequestFloorPlanDoorSettingsEvent>();
			collection.AddSingleton<IAsyncPacket, FloorPlanSaveEvent>();
			collection.AddSingleton<IAsyncPacket, RequestRoomWordFilterEvent>();
			collection.AddSingleton<IAsyncPacket, RoomWordFilterModifyEvent>();
        }
    }
}
