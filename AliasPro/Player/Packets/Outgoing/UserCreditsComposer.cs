namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserCreditsComposer : IPacketComposer
    {
        private readonly int _credits;

        public UserCreditsComposer(int credits)
        {
            _credits = credits;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserCreditsMessageComposer);
            message.WriteString(_credits + ".0");
            return message;
        }
    }
}
