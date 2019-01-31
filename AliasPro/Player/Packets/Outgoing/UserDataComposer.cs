namespace AliasPro.Player.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserDataComposer : ServerPacket
    {
        public UserDataComposer(IPlayer player)
            : base(Outgoing.UserDataMessageComposer)
        {
            WriteInt(player.Id);
            WriteString(player.Username);
            WriteString(player.Figure);
            WriteString(player.Gender);
            WriteString(player.Motto);
            WriteString("");
            WriteBoolean(false);
            WriteInt(0); //Respect
            WriteInt(0); //DailyRespect
            WriteInt(0); //DailyPetRespect
            WriteBoolean(false); //Friendstream
            WriteString("01.01.1970 00:00:00"); //Last online
            WriteBoolean(false); //Can change name
            WriteBoolean(false);
            //todo:
        }
    }
}
