using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;

    public class RequestFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFriendsMessageEvent;

        public Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //not used??
            return Task.CompletedTask;
        }
    }
}
