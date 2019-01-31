namespace AliasPro.Player.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserPerksComposer : ServerPacket
    {
        public UserPerksComposer(IPlayer player)
            : base(Outgoing.UserPerksMessageComposer)
        {
            WriteInt(15);
            WriteString("USE_GUIDE_TOOL");
            WriteString("");
            WriteBoolean(false);
            WriteString("GIVE_GUIDE_TOURS");
            WriteString("requirement.unfulfilled.helper_le");
            WriteBoolean(false);
            WriteString("JUDGE_CHAT_REVIEWS");
            WriteString("");
            WriteBoolean(true);
            WriteString("VOTE_IN_COMPETITIONS");
            WriteString("");
            WriteBoolean(true);
            WriteString("CALL_ON_HELPERS");
            WriteString("");
            WriteBoolean(false);
            WriteString("CITIZEN");
            WriteString("");
            WriteBoolean(true);
            WriteString("TRADE");
            WriteString("");
            WriteBoolean(true);
            WriteString("HEIGHTMAP_EDITOR_BETA");
            WriteString("");
            WriteBoolean(false);
            WriteString("BUILDER_AT_WORK");
            WriteString("");
            WriteBoolean(true);
            WriteString("NAVIGATOR_PHASE_ONE_2014");
            WriteString("");
            WriteBoolean(false);
            WriteString("CAMERA");
            WriteString("");
            WriteBoolean(true);
            WriteString("NAVIGATOR_PHASE_TWO_2014");
            WriteString("");
            WriteBoolean(true);
            WriteString("MOUSE_ZOOM");
            WriteString("");
            WriteBoolean(true);
            WriteString("NAVIGATOR_ROOM_THUMBNAIL_CAMERA");
            WriteString("");
            WriteBoolean(true);
            WriteString("HABBO_CLUB_OFFER_BETA");
            WriteString("");
            WriteBoolean(true);
        }
    }
}
