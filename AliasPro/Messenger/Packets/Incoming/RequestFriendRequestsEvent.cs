using AliasPro.Messenger.Packets.Outgoing;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

    public class RequestFriendRequestsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFriendRequestsMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new LoadFriendRequestsComposer(session.Player.Messenger.Requests));
        }
    }
}
