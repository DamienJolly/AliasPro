namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class NavigatorCollapsedCategoriesComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorCollapsedCategoriesMessageComposer);
            message.WriteInt(46);
            message.WriteString("new_ads");
            message.WriteString("friend_finding");
            message.WriteString("staffpicks");
            message.WriteString("with_friends");
            message.WriteString("with_rights");
            message.WriteString("query");
            message.WriteString("recommended");
            message.WriteString("my_groups");
            message.WriteString("favorites");
            message.WriteString("history");
            message.WriteString("top_promotions");
            message.WriteString("campaign_target");
            message.WriteString("friends_rooms");
            message.WriteString("groups");
            message.WriteString("metadata");
            message.WriteString("history_freq");
            message.WriteString("highest_score");
            message.WriteString("competition");
            message.WriteString("category__Agencies");
            message.WriteString("category__Role Playing");
            message.WriteString("category__Global Chat & Discussi");
            message.WriteString("category__GLOBAL BUILDING AND DE");
            message.WriteString("category__global party");
            message.WriteString("category__global games");
            message.WriteString("category__global fansite");
            message.WriteString("category__global help");
            message.WriteString("category__Trading");
            message.WriteString("category__global personal space");
            message.WriteString("category__Player Life");
            message.WriteString("category__TRADING");
            message.WriteString("category__global official");
            message.WriteString("category__global trade");
            message.WriteString("category__global reviews");
            message.WriteString("category__global bc");
            message.WriteString("category__global personal space");
            message.WriteString("eventcategory__Hottest Events");
            message.WriteString("eventcategory__Parties & Music");
            message.WriteString("eventcategory__Role Play");
            message.WriteString("eventcategory__Help Desk");
            message.WriteString("eventcategory__Trading");
            message.WriteString("eventcategory__Games");
            message.WriteString("eventcategory__Debates & Discuss");
            message.WriteString("eventcategory__Grand Openings");
            message.WriteString("eventcategory__Friending");
            message.WriteString("eventcategory__Jobs");
            message.WriteString("eventcategory__Group Events");
            return message;
        }
    }
}
