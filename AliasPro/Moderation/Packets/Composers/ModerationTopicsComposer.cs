using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationTopicsComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationTopicsMessageComposer);
			message.WriteInt(1);
			message.WriteString("Sexually Explicit");
			message.WriteInt(2);
			message.WriteString("test");
			message.WriteInt(1);
			message.WriteString("testing");
			message.WriteString("test2");
			message.WriteInt(2);
			message.WriteString("testing2");
			return message;
        }
    }
}
