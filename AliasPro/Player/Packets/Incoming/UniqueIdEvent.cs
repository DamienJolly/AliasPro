using AliasPro.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    public class UniqueIdEvent : IAsyncPacket
    {
        public short Header { get; } = IncomingHeaders.UniqueIdMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            clientPacket.ReadString();
            string uniqueId = clientPacket.ReadString();
            session.UniqueId = uniqueId;
            System.Console.WriteLine("Session Id: " + session.UniqueId);
            //todo: await send composer
        }
    }
}
