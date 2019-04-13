using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserProfileComposer : IPacketComposer
    {
        private readonly IPlayer _player;

        public UserProfileComposer(IPlayer player)
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
            message.WriteInt(1337); //Achievment Score
            message.WriteInt(0); ///Friend count
            message.WriteBoolean(false); //Is friend
            message.WriteBoolean(false); //Has requested
            message.WriteBoolean(_player.Online);
            message.WriteInt(0); //Groups
            message.WriteInt(0); //Last online (seconds)
            message.WriteBoolean(true); //?
            return message;
        }
    }
}
