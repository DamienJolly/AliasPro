using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Catalog.Packets.Composers
{
    public class RecyclerCompleteComposer : IPacketComposer
    {
        public static int RECYCLING_COMPLETE = 1;
        public static int RECYCLING_CLOSED = 2;

        private readonly int _code;

        public RecyclerCompleteComposer(int code)
        {
            _code = code;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RecyclerCompleteMessageComposer);
            message.WriteInt(_code);
            message.WriteInt(0); // prizeId, lol why habbo?
            return message;
        }
    }
}
