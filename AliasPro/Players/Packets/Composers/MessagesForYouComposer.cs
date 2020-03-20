using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class MessagesForYouComposer : IMessageComposer
    {
        private readonly string[] _messages;

        public MessagesForYouComposer(string[] messages)
        {
            _messages = messages;
        }

        public MessagesForYouComposer(string message)
        {
            _messages = new[] { message };
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.MessagesForYouMessageComposer);
            message.WriteInt(_messages.Length);

            foreach (string msg in _messages)
                message.WriteString(msg);

            return message;
        }
    }
}
