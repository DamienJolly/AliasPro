using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Game.Catalog.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Game.Catalog.Packets.Events
{
    public class RequestDiscountEvent : IMessageEvent
    {
        public short Header => Incoming.RequestDiscountMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            await session.SendPacketAsync(new DiscountComposer());
        }
    }
}
