using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationTopicsComposer : IPacketComposer
    {
        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModerationTopicsMessageComposer);
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
