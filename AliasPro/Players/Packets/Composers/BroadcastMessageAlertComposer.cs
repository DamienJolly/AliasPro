using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class BroadcastMessageAlertComposer : IMessageComposer
    {
        private readonly string _message;
        private readonly string _link;

        public BroadcastMessageAlertComposer(string message, string link)
        {
            _message = message;
            _link = link;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.BroadcastMessageAlertMessageComposer);
            message.WriteString(_message);
            message.WriteString(_link);
            return message;
        }
    }
}
