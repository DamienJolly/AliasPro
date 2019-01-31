namespace AliasPro.Player.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserRightsComposer : ServerPacket
    {
        public UserRightsComposer(IPlayer player)
            : base(Outgoing.UserRightsMessageComposer)
        {
            WriteInt(2); //todo: subscription
            WriteInt(player.Rank);
            WriteBoolean(false); //todo: ambassador
        }
    }
}
