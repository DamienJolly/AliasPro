using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationReportReceivedAlertComposer : IMessageComposer
    {
        public static int REPORT_RECEIVED = 0;
        public static int REPORT_WINDOW = 1;
        public static int REPORT_ABUSIVE = 2;

        private readonly int _code;
        private readonly string _message;
        
        public ModerationReportReceivedAlertComposer(int code, string message)
        {
            _code = code;
            _message = message;
        }

		public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationReportReceivedAlertMessageComposer);
            message.WriteInt(_code);
            message.WriteString(_message);
            return message;
        }
    }
}
