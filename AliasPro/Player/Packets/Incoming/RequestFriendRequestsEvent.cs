using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;

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
