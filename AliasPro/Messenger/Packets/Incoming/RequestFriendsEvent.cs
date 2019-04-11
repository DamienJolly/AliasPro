using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

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
