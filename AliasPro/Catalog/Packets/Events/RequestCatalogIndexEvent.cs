using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestCatalogIndexEvent : IMessageEvent
    {
        public short Header => Incoming.RequestCatalogIndexMessageEvent;

        public Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            // not used?
            return Task.CompletedTask;
        }
    }
}
