namespace AliasPro.Player.Packets.Outgoing
{
    using AliasPro.API.Player.Models;
    using AliasPro.Player.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
