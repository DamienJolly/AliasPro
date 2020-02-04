using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class RecyclerCompleteComposer : IMessageComposer
    {
        public static int RECYCLING_COMPLETE = 1;
        public static int RECYCLING_CLOSED = 2;

        private readonly int _code;

        public RecyclerCompleteComposer(int code)
        {
            _code = code;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RecyclerCompleteMessageComposer);
            message.WriteInt(_code);
            message.WriteInt(0); // prizeId, lol why habbo?
            return message;
        }
    }
}
