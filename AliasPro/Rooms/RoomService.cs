using AliasPro.API.Rooms;
using AliasPro.Communication.Messages;
using AliasPro.Rooms.Packets.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AliasPro.Rooms
{
    internal class RoomService : IService
    {
        public void Register(IServiceCollection collection)
        {
            collection.AddSingleton<RoomDao>();
            collection.AddSingleton<IRoomController, RoomController>();

            RegisterPackets(collection);
        }

        private static void RegisterPackets(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageEvent, RequestRoomLoadEvent>();
            collection.AddSingleton<IMessageEvent, RequestRoomEntryDataEvent>();
            collection.AddSingleton<IMessageEvent, RequestFurnitureAliasesEvent>();
            collection.AddSingleton<IMessageEvent, MoveAvatarEvent>();
            collection.AddSingleton<IMessageEvent, AvatarChatEvent>();
            collection.AddSingleton<IMessageEvent, AvatarShoutEvent>();
            collection.AddSingleton<IMessageEvent, AvatarWhisperEvent>();
            collection.AddSingleton<IMessageEvent, RequestRoomDataEvent>();
            collection.AddSingleton<IMessageEvent, RequestCreateRoomEvent>();
            collection.AddSingleton<IMessageEvent, RequestRoomSettingsEvent>();
            collection.AddSingleton<IMessageEvent, RoomSettingsSaveEvent>();
            collection.AddSingleton<IMessageEvent, UserStartTypingEvent>();
            collection.AddSingleton<IMessageEvent, UserStopTypingEvent>();
            collection.AddSingleton<IMessageEvent, UserLookAtPointEvent>();
            collection.AddSingleton<IMessageEvent, UserDanceEvent>();
            collection.AddSingleton<IMessageEvent, UserActionEvent>();
            collection.AddSingleton<IMessageEvent, UserSitEvent>();
            collection.AddSingleton<IMessageEvent, UserRemoveRightsEvent>();
            collection.AddSingleton<IMessageEvent, UserGiveRightsEvent>();
            collection.AddSingleton<IMessageEvent, WiredTriggerSaveDataEvent>();
            collection.AddSingleton<IMessageEvent, WiredEffectSaveDataEvent>();
            collection.AddSingleton<IMessageEvent, WiredConditionSaveDataEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserPlaceBotEvent>();
			collection.AddSingleton<IMessageEvent, RoomBotSettingsEvent>();
			collection.AddSingleton<IMessageEvent, RoomBotSaveSettingsEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserPickupBotEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserPlacePetEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserPickupPetEvent>();
			collection.AddSingleton<IMessageEvent, RequestPetInformationEvent>();
			collection.AddSingleton<IMessageEvent, RequestFloorPlanBlockedTilesEvent>();
			collection.AddSingleton<IMessageEvent, RequestFloorPlanDoorSettingsEvent>();
			collection.AddSingleton<IMessageEvent, FloorPlanSaveEvent>();
			collection.AddSingleton<IMessageEvent, RequestRoomWordFilterEvent>();
			collection.AddSingleton<IMessageEvent, RoomWordFilterModifyEvent>();
			collection.AddSingleton<IMessageEvent, RequestPromotionRoomsEvent>();
			collection.AddSingleton<IMessageEvent, BuyRoomPromotionEvent>();
			collection.AddSingleton<IMessageEvent, UpdateRoomPromotionEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserSignEvent>();
			collection.AddSingleton<IMessageEvent, RoomMuteEvent>();
			collection.AddSingleton<IMessageEvent, SetHomeRoomEvent>();
			collection.AddSingleton<IMessageEvent, RequestRoomRightsEvent>();
			collection.AddSingleton<IMessageEvent, RoomRemoveAllRightsEvent>();
			collection.AddSingleton<IMessageEvent, RoomRemoveRightsEvent>();
			collection.AddSingleton<IMessageEvent, IgnoreRoomUserEvent>();
			collection.AddSingleton<IMessageEvent, UnIgnoreRoomUserEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserKickEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserMuteEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserBanEvent>();
			collection.AddSingleton<IMessageEvent, RoomUserUnbanEvent>();
			collection.AddSingleton<IMessageEvent, RequestBannedUsersEvent>();
			collection.AddSingleton<IMessageEvent, HandleDoorbellEvent>();
			collection.AddSingleton<IMessageEvent, RequestRoomLoadDataEvent>();
        }
    }
}
