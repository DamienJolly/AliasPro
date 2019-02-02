namespace AliasPro.Landing.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class HotelViewDataComposer : IPacketComposer
    {
        private readonly string _data;
        private readonly string _key;

        public HotelViewDataComposer(string data, string key)
        {
            _data = data;
            _key = key;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.HotelViewDataMessageComposer);
            message.WriteString(_data);
            message.WriteString(_key);
            return message;
        }
    }
}
