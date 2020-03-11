using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;

namespace AliasPro.Players.Packets.Composers
{
    public class UserDataComposer : IMessageComposer
    {
        private readonly IPlayer _player;

        public UserDataComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserDataMessageComposer);
            message.WriteInt((int)_player.Id);
            message.WriteString(_player.Username);
            message.WriteString(_player.Figure);
            message.WriteString(_player.Gender == PlayerGender.MALE ? "m" : "f");
            message.WriteString(_player.Motto);
            message.WriteString("");
            message.WriteBoolean(false);
            message.WriteInt(_player.RespectsRecieved); //Respect
            message.WriteInt(_player.Respects);
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
