using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.Players.Packets.Composers
{
    public class UserDataComposer : IPacketComposer
    {
        private readonly IPlayer _player;

        public UserDataComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserDataMessageComposer);
            message.WriteInt(_player.Id);
            message.WriteString(_player.Username);
            message.WriteString(_player.Figure);
            message.WriteString(_player.Gender == PlayerGender.MALE ? "m" : "f");
            message.WriteString(_player.Motto);
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteInt(0); //Respect
            message.WriteInt(0); //DailyRespect
            message.WriteInt(0); //DailyPetRespect
            message.WriteBoolean(false); //Friendstream
            message.WriteString("01.01.1970 00:00:00"); //Last online
            message.WriteBoolean(false); //Can change name
            message.WriteBoolean(false);
            //todo:
            return message;
        }
    }
}
