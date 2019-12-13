namespace AliasPro.Network.Events.Headers
{
    internal static class Incoming
    {
        // Players
        internal const short UniqueIdMessageEvent = 3465;
        internal const short SecureLoginMessageEvent = 1930;
        internal const short RequestUserDataMessageEvent = 3092;
        internal const short RequestUserCreditsMessageEvent = 2109;
        internal const short RequestUserProfileMessageEvent = 2023;
        internal const short RequestMeMenuSettingsMessageEvent = 3537;
        internal const short SaveUserVolumesMessageEvent = 2507;
        internal const short SavePreferOldChatMessageEvent = 2213;
        internal const short SaveIgnoreRoomInvitesMessageEvent = 3110;
        internal const short SaveBlockCameraFollowMessageEvent = 191;

        // Badges
        internal const short RequestBadgeInventoryMessageEvent = 1023;
        internal const short UserWearBadgeMessageEvent = 3804;
        internal const short RequestWearingBadgesMessageEvent = 1318;

        // Figure
        internal const short UpdateFigureMessageEvent = 1588;
        internal const short RequestUserWardrobeMessengerEvent = 3960;
        internal const short SaveWardrobeMessageEvent = 2020;

        // Messenger
        internal const short RequestInitFriendsMessageEvent = 1405;
        internal const short RequestFriendRequestsMessageEvent = 2467;
        internal const short FriendRequestMessageEvent = 3779;
        internal const short RequestFriendsMessageEvent = 1368;
        internal const short AcceptFriendRequestMessageEvent = 1879;
        internal const short DeclineFriendRequestMessageEvent = 204;
        internal const short RemoveFriendMessageEvent = 853;
        internal const short SearchUserMessageEvent = 3483;
        internal const short FriendPrivateMessageEvent = 2279;
        internal const short RoomInviteMessageEvent = 1280;
        internal const short RequestProfileFriendsMessageEvent = 3763;
        internal const short ChangeRelationMessageEvent = 2356;

        // Items
        internal const short RequestFurniInventoryMessageEvent = 2835;
        internal const short PlaceItemMessageEvent = 2187;
        internal const short UpdateItemMessageEvent = 1620;
        internal const short RemoveItemMessageEvent = 3036;
        internal const short UpdateWallMessageEvent = 1132;
        internal const short ToggleFloorItemMessageEvent = 1699;
        internal const short ToggleWallItemMessageEvent = 2100;
        internal const short TriggerDiceMessageEvent = 237;
		internal const short ToggleOneWayMessageEvent = 2192;
		internal const short CloseDiceMessageEvent = 1285;
		internal const short RedeemItemMessageEvent = 1018;
		internal const short LoveLockConfirmMessageEvent = 1600;
		internal const short SetStackToolHeightMessageEvent = 2169;
		internal const short RedeemClothingMessageEvent = 687;
		internal const short RedeemGiftMessageEvent = 173;

		// Catalog
		internal const short RequestDiscountMessageEvent = 703;
        internal const short RequestCatalogIndexMessageEvent = 1191;
        internal const short RequestCatalogModeMessageEvent = 2565;
        internal const short RequestCatalogPageMessageEvent = 3547;
        internal const short CatalogBuyItemMessageEvent = 2687;
        internal const short CatalogBuyGiftItemMessageEvent = 2649;
        internal const short RequestGiftConfigurationMessageEvent = 3493;

        // Rooms
        internal const short RequestRoomLoadMessageEvent = 3464;
        internal const short RequestRoomEntryDataMessageEvent = 1583;
        internal const short RequestFurnitureAliasesMessageEvent = 2443;
        internal const short MoveAvatarMessageEvent = 3155;
        internal const short AvatarChatMessageEvent = 500;
        internal const short RequestRoomDataMessageEvent = 425;
        internal const short FindNewFriendsMessageEvent = 1100;
        internal const short RequestCreateRoomMessageEvent = 1227;
        internal const short RequestRoomSettingsMessageEvent = 923;
        internal const short RoomSettingsSaveMessageEvent = 1820;
        internal const short UserStartTypingMessageEvent = 3697;
        internal const short UserStopTypingMessageEvent = 2462;
        internal const short UserLookAtPointMessageEvent = 258;
        internal const short UserDanceMessageEvent = 3911;
        internal const short UserActionMessageEvent = 2925;
        internal const short UserSitMessageEvent = 68;
        internal const short UserGiveRightsMessageEvent = 3880;
        internal const short UserRemoveRightsMessageEvent = 404;
        internal const short WiredTriggerSaveDataMessageEvent = 3810;
        internal const short WiredEffectSaveDataMessageEvent = 2719;
        internal const short WiredConditionSaveDataMessageEvent = 166;
		internal const short RoomUserPlaceBotMessageEvent = 3978;
		internal const short RoomBotSettingsMessageEvent = 2714;
		internal const short RoomBotSaveSettingsMessageEvent = 1676;
		internal const short RoomUserPickupBotMessageEvent = 389;
		internal const short RoomUserPlacePetMessageEvent = 1061;
		internal const short RoomUserPickupPetMessageEvent = 1946;
		internal const short RequestPetInformationMessageEvent = 3352;
        internal const short RequestFloorPlanDoorSettingsMessageEvent = 2314;
        internal const short RequestFloorPlanBlockedTilesMessageEvent = 24;
        internal const short FloorPlanSaveMessageEvent = 40;
        internal const short RequestRoomWordFilterMessageEvent = 3571;
        internal const short RoomWordFilterModifyMessageEvent = 3709;
        internal const short RequestPromotionRoomsMessageEvent = 3802;

        // Landing
        internal const short HotelViewDataMessageEvent = 1579;
        internal const short RequestNewsListMessageEvent = 1156;
        internal const short HotelViewMessageEvent = 3119;

        // Navigator
        internal const short InitializeNavigatorMessageEvent = 2142;
        internal const short RequestUserFlatCatsMessageEvent = 3976;
        internal const short RequestNavigatorFlatsMessageEvent = 708;
        internal const short NavigatorSearchMessageEvent = 3612;
        internal const short UpdateNavigatorPreferencesMessageEvent = 107;

		// Moderation
		internal const int ModerationMessageMessageEvent = 1932;
		internal const int ModerationAlertMessageEvent = 2026;
		internal const int ModerationTradeLockMessageEvent = 57;
		internal const int ModerationMuteMessageEvent = 1034;
		internal const int ModerationBanMessageEvent = 2429;
		internal const int ModerationPickTicketMessageEvent = 3388;
        internal const int ModerationReleaseTicketMessageEvent = 263;
        internal const int ModerationCloseTicketMessageEvent = 1383;
		internal const int ModerationKickMessageEvent = 472;
		internal const int ModerationRequestRoomChatlogMessageEvent = 891;
        internal const int ModerationRequestIssueChatlogMessageEvent = 2986;
        internal const int ModerationRequestRoomInfoMessageEvent = 306;
        internal const int ModerationRoomAlertMessageEvent = 557;
        internal const int ModerationChangeRoomSettingsMessageEvent = 124;
        internal const int ModerationRequestUserInfoMessageEvent = 3939;
        internal const int ModerationRequestRoomVisitsMessageEvent = 3371;
        internal const int ModerationRequestUserChatlogMessageEvent = 1362;

		// Achievements
		internal const int RequestAchievementsMessageEvent = 3280;

		// Groups
		internal const int RequestGroupInfoMessageEvent = 2873;
		internal const int RequestGroupBuyRoomsMessageEvent = 2627;
		internal const int RequestGroupPartsMessageEvent = 1252;
		internal const int RequestGroupBuyMessageEvent = 730;
		internal const int RequestGroupMembersMessageEvent = 3272;
		internal const int GroupSetAdminMessageEvent = 3830;
		internal const int GroupRemoveAdminMessageEvent = 438;
		internal const int RequestGroupJoinMessageEvent = 3314;
		internal const int GroupRemoveMemberMessageEvent = 489;
		internal const int GroupAcceptMembershipMessageEvent = 1624;
		internal const int GroupDeclineMembershipMessageEvent = 670;
		internal const int RequestGroupManageMessageEvent = 601;
		internal const int GroupChangeBadgeMessageEvent = 172;
		internal const int GroupChangeColorsMessageEvent = 175;
		internal const int GroupChangeNameDescMessageEvent = 629;
		internal const int GroupChangeSettingsMessageEvent = 332;
		internal const int GroupDeleteMessageEvent = 3492;

		// Subscription
		internal const int RequestUserClubMessageEvent = 3796;

		// Trading
		internal const int TradeStartMessageEvent = 3586;
		internal const int TradeOfferItemMessageEvent = 2175;
		internal const int TradeOfferMultipleItemsMessageEvent = 1725;
		internal const int TradeCancelOfferItemMessageEvent = 2177;
		internal const int TradeAcceptMessageEvent = 999;
		internal const int TradeUnAcceptMessageEvent = 2851;
		internal const int TradeCloseMessageEvent = 3602;
		internal const int TradeConfirmMessageEvent = 727;

		// Bots
		internal const int RequestBotInventoryMessageEvent = 1278;
		internal const int RequestPetInventoryMessageEvent = 86;

		// Pets
		internal const int RequestPetBreedsMessageEvent = 3072;
		internal const int CheckPetNameMessageEvent = 2330;
	}
}
