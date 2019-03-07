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

        // Badges
        internal const short RequestBadgeInventoryMessageEvent = 1023;
        internal const short UserWearBadgeMessageEvent = 3804;
        internal const short RequestWearingBadgesMessageEvent = 1318;

        // Figure
        internal const short UpdateFigureMessageEvent = 1588;

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

        // Items
        internal const short RequestFurniInventoryMessageEvent = 2835;
        internal const short PlaceItemMessageEvent = 2187;
        internal const short UpdateItemMessageEvent = 1620;
        internal const short RemoveItemMessageEvent = 3036;
        internal const short UpdateWallMessageEvent = 1132;
        internal const short ToggleFloorItemMessageEvent = 1699;
        internal const short ToggleWallItemMessageEvent = 2100;

        // Catalog
        internal const short RequestDiscountMessageEvent = 703;
        internal const short RequestCatalogIndexMessageEvent = 1191;
        internal const short RequestCatalogModeMessageEvent = 2565;
        internal const short RequestCatalogPageMessageEvent = 3547;
        internal const short CatalogBuyItemMessageEvent = 2687;

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
    }
}
