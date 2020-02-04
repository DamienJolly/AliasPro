using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class UserClubComposer : IMessageComposer
    {
        public UserClubComposer()
        {

        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserClubMessageComposer);
			message.WriteString("habbo_club");
			message.WriteInt(0); //display days
			message.WriteInt(2);
			message.WriteInt(0); //display months
			message.WriteInt(1);
			message.WriteBoolean(true); // hc
			message.WriteBoolean(true); // vip
			message.WriteInt(0);
			message.WriteInt(0);
			message.WriteInt(495);
            return message;
        }
    }
}
