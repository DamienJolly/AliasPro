using AliasPro.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Player.Packets.Outgoing;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    public class SecureLoginEvent : IAsyncPacket
    {
        public short Header { get; } = IncomingHeaders.SecureLoginMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string ssoTicket = clientPacket.ReadString();
            await session.WriteAndFlushAsync(new SecureLoginOKComposer());
        }
    }
}
