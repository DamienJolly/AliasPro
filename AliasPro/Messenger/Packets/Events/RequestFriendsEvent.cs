using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestFriendsMessageEvent;

        public void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //not used??
        }
    }
}
