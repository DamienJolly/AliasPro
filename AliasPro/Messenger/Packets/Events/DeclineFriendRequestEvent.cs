using AliasPro.API.Messenger;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Events
{
    public class DeclineFriendRequestEvent : IMessageEvent
    {
        public short Header => Incoming.DeclineFriendRequestMessageEvent;

        private readonly IMessengerController _messengerController;

        public DeclineFriendRequestEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            bool DeclineAll = message.ReadBoolean();
            
            if(DeclineAll)
            {
                session.Player.Messenger.RemoveAllRequests();
                await _messengerController.RemoveAllRequestsAsync(session.Player.Id);
            }
            else
            {
                message.ReadInt(); // dunno?
                uint targetId = (uint)message.ReadInt();

                session.Player.Messenger.RemoveRequest(targetId);
                await _messengerController.RemoveRequestAsync(session.Player.Id, targetId);
            }
        }
    }
}
