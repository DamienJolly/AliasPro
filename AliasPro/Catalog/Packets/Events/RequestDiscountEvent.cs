using AliasPro.API.Sessions.Models;
using AliasPro.Catalog.Packets.Composers;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Catalog.Packets.Events
{
    public class RequestDiscountEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.RequestDiscountMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new DiscountComposer());
        }
    }
}
