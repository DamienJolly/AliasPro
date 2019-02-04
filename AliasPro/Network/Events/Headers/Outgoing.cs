namespace AliasPro.Network.Events.Headers
{
    internal static class Outgoing
    {
        // Players
        internal const short SecureLoginOKMessageComposer = 3412;
        internal const short HomeRoomMessageComposer = 908;
        internal const short UserRightsMessageComposer = 2946;
        internal const short AvailabilityStatusMessageComposer = 770;
        internal const short UserPerksMessageComposer = 1105;
        internal const short UserDataMessageComposer = 3862;
        internal const short FurniListMessageComposer = 826;

        // Items
        internal const short ObjectAddMessageComposer = 2396;
        internal const short RemovePlayerItemMessageComposer = 1518;
        internal const short FloorItemUpdateMessageComposer = 854;
        internal const short RemoveFloorItemMessageComposer = 2986;
        internal const short AddPlayerItemsMessageComposer = 2323;
        internal const short InventoryRefreshMessageComposer = 32;

        // Catalog
        internal const short DiscountMessageComposer = 1602;

        // Rooms
        internal const short RoomOpenMessageComposer = 1975;
        internal const short RoomModelMessageComposer = 1303;
        internal const short RoomScoreMessageComposer = 2010;
        internal const short HeightMapMessageComposer = 322;
        internal const short FloorHeightMapMessageComposer = 1909;
        internal const short RoomEntryInfoMessageComposer = 521;
        internal const short RoomVisualizationSettingsMessageComposer = 2244;
        internal const short FurnitureAliasesMessageComposer = 2958;
        internal const short EntitiesMessageComposer = 633;
        internal const short EntityUpdateMessageComposer = 3902;
        internal const short AvatarChatMessageComposer = 3576;
        internal const short RoomDataMessageComposer = 439;
        internal const short RoomFloorItemsMessageComposer = 2585;

        // Landing
        internal const short HotelViewDataMessageComposer = 730;
        internal const short HallOfFameMessageComposer = 1243;
        internal const short NewsListMessageComposer = 811;

        // Navigator
        internal const short NavigatorMetaDataParserMessageComposer = 2279;
        internal const short NavigatorLiftedRoomsMessageComposer = 2525;
        internal const short NavigatorCollapsedCategoriesMessageComposer = 2141;
        internal const short NavigatorPreferencesMessageComposer = 607;
        internal const short UserFlatCatsMessageComposer = 1084;
        internal const short NavigatorFlatCatsMessageComposer = 3910;
        internal const short NavigatorSearchResultSetMessageComposer = 1388;
    }
}
