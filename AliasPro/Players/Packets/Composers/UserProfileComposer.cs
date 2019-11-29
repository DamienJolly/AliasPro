using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserProfileComposer : IPacketComposer
    {
        private readonly IPlayerData _player;

        public UserProfileComposer(IPlayerData player)
        {
            _player = player;
        }

        public ServerPacket Compose()
        {
            //todo:
            ServerPacket message = new ServerPacket(Outgoing.UserProfileMessageComposer);
            message.WriteInt(_player.Id);
            message.WriteString(_player.Username);
            message.WriteString(_player.Figure);
            message.WriteString(_player.Motto);
            message.WriteString("01.01.1970 00:00:00"); //Account created
            message.WriteInt(_player.Score);
            message.WriteInt(0); ///Friend count
            message.WriteBoolean(false); //Is friend
            message.WriteBoolean(false); //Has requested
            message.WriteBoolean(_player.Online);

            message.WriteInt(1); //Groups
			{
				message.WriteInt(1); // group id
				message.WriteString("Testing"); // group name
				message.WriteString(""); // group badge
				message.WriteString(""); // group colour1
				message.WriteString(""); // group colour2
				message.WriteBoolean(true); // Fav. group
				message.WriteInt(1); // group owner id
				message.WriteBoolean(true); // is owner
			}


			message.WriteInt(0); //Last online (seconds)
            message.WriteBoolean(true); //?
            return message;
        }
    }
}
