using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserClubComposer : IPacketComposer
    {
        public UserClubComposer()
        {

        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserClubMessageComposer);
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
