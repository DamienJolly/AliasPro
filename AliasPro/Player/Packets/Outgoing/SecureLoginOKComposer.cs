namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class SecureLoginOKComposer : ServerPacket
    {
        public SecureLoginOKComposer()
            : base(Outgoing.SecureLoginOKMessageComposer)
        {
        }
    }
}
