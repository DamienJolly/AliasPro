namespace AliasPro.Landing.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;

    public class HotelViewDataComposer : ServerPacket
    {
        public HotelViewDataComposer(string data, string key)
            : base(Outgoing.HotelViewDataMessageComposer)
        {
            WriteString(data);
            WriteString(key);
        }
    }
}
