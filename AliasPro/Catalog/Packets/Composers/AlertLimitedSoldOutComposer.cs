using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class AlertLimitedSoldOutComposer : IMessageComposer
    {
        public ServerMessage Compose() =>
            new ServerMessage(Outgoing.AlertLimitedSoldOutMessageComposer);
    }
}
