using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationIssueHandledComposer : IMessageComposer
    {
        private readonly int _code = 0;
        private readonly string _message = "";
        
        public ModerationIssueHandledComposer(int code)
        {
            _code = code;
        }

		public ModerationIssueHandledComposer(string message)
		{
			_message = message;
		}

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationIssueHandledMessageComposer);
            message.WriteInt(_code);
            message.WriteString(_message);
            return message;
        }
    }
}
