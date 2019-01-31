namespace AliasPro.Player.Packets.Outgoing
{
    using Models;
    using Network.Events.Headers;
    using Network.Protocol;

    public class AvailabilityStatusComposer : ServerPacket
    {
        public AvailabilityStatusComposer()
            : base(Outgoing.AvailabilityStatusMessageComposer)
        {
            WriteBoolean(true);
            WriteBoolean(false);
            WriteBoolean(true);
        }
    }
}
