using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class UserPerksComposer : IMessageComposer
    {
        private readonly IPlayer _player;

        public UserPerksComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserPerksMessageComposer);
            message.WriteInt(15);
            message.WriteString("USE_GUIDE_TOOL");
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteString("GIVE_GUIDE_TOURS");
            message.WriteString("requirement.unfulfilled.helper_le");
            message.WriteBoolean(false);
            message.WriteString("JUDGE_CHAT_REVIEWS");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("VOTE_IN_COMPETITIONS");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("CALL_ON_HELPERS");
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteString("CITIZEN");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("TRADE");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("HEIGHTMAP_EDITOR_BETA");
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteString("BUILDER_AT_WORK");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("NAVIGATOR_PHASE_ONE_2014");
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteString("CAMERA");
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteString("NAVIGATOR_PHASE_TWO_2014");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("MOUSE_ZOOM");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("NAVIGATOR_ROOM_THUMBNAIL_CAMERA");
            message.WriteString("");
            message.WriteBoolean(true);
            message.WriteString("HABBO_CLUB_OFFER_BETA");
            message.WriteString("");
            message.WriteBoolean(true);
            return message;
        }
    }
}
