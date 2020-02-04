using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Landing.Packets.Composers
{
    public class HotelViewDataComposer : IMessageComposer
    {
        private readonly string _data;
        private readonly string _key;

        public HotelViewDataComposer(string data, string key)
        {
            _data = data;
            _key = key;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.HotelViewDataMessageComposer);
            message.WriteString(_data);
            message.WriteString(_key);
            return message;
        }
    }
}
