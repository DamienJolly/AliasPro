using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Messenger.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class RequestFriendRequestsEvent : IMessageEvent
    {
        public short Header => Incoming.RequestFriendRequestsMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new LoadFriendRequestsComposer(session.Player.Messenger.Requests));
        }
    }
}
